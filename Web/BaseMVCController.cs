using GenericFunctions.AccessLog;
using GenericFunctions.Mapping;
using GenericFunctions.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GenericFunctions.Web
{
    public class BaseMVCController : Controller
    {
        public ILogger Logger { protected get; set; }
        public IObjectMapper Mapper { protected get; set; }

        public IAccessLogger accessLogger { protected get; set; }

        protected string RequestUrl
        {
            get
            {
                string url = string.Concat(this.Request.Scheme, "://", this.Request.Host, this.Request.Path, this.Request.QueryString);
                return url;
            }
        }

        public BaseMVCController(ILogger logger)
        {
            Logger = logger;
        }

        public BaseMVCController(ILogger logger, IAccessLogger accessLogger)
        {
            Logger = logger;
            this.accessLogger = accessLogger;
        }

        public BaseMVCController(ILogger logger, IObjectMapper mapper)
        {
            Logger = logger;
            this.Mapper = mapper;
        }

        public IActionResult HandleException(object model, Exception ex, bool partial = false)
        {
            return HandleException(null, model, ex, partial);
        }

        public IActionResult HandleException(Exception ex, bool partial = false)
        {
            return HandleException(null, null, ex, partial);
        }

        public IActionResult HandleException(string viewName, Exception ex, bool partial = false)
        {
            return HandleException(viewName, null, ex, partial);
        }

        public IActionResult HandleException(string viewName, object model, Exception ex, bool partial = false)
        {
            Logger.LogError(ex, ex.Message);
            var problemDetails = ExceptionsMapping.Map(ex);
            problemDetails.StackTrace = ex.StackTrace;
            ViewData["ProblemDetails"] = problemDetails;

            if (string.IsNullOrEmpty(viewName))
            {

                if (partial)
                    return PartialView(viewName, model);

                return View(viewName, model);
            }

            if (partial)
                return PartialView(model);

            return View(model);
        }

        protected bool IsAjaxRequest()
        {
            if (Request.Headers != null)
                return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        protected virtual string RenderViewComponentToString(string componentName, object arguments = null)
        {
            //original implementation: https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.ViewFeatures/Internal/ViewComponentResultExecutor.cs
            //we customized it to allow running from controllers

            if (string.IsNullOrEmpty(componentName))
                throw new ArgumentNullException(nameof(componentName));

            var actionContextAccessor = HttpContext.RequestServices.GetService(typeof(IActionContextAccessor)) as IActionContextAccessor;
            if (actionContextAccessor == null)
                throw new Exception("IActionContextAccessor cannot be resolved");

            var context = actionContextAccessor.ActionContext;

            var viewComponentResult = ViewComponent(componentName, arguments);

            var viewData = ViewData;
            if (viewData == null)
            {
                throw new NotImplementedException();
            }

            var tempData = TempData;
            if (tempData == null)
            {
                throw new NotImplementedException();
            }

            using var writer = new StringWriter();
            var viewContext = new ViewContext(
                context,
                null,
                viewData,
                tempData,
                writer,
                new HtmlHelperOptions());

            // IViewComponentHelper is stateful, we want to make sure to retrieve it every time we need it.
            var viewComponentHelper = context.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);

            var result = viewComponentResult.ViewComponentType == null ?
                viewComponentHelper.InvokeAsync(viewComponentResult.ViewComponentName, viewComponentResult.Arguments) :
                viewComponentHelper.InvokeAsync(viewComponentResult.ViewComponentType, viewComponentResult.Arguments);

            result.Result.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        public async Task SaveAccessLogAsync(string actionName, string entityName, int entityId, string subEntityName, int subEntityId, string rootEntityName, int rootEntityId)
        {
            try
            {
                await accessLogger.SaveAsync(actionName, entityName, entityId, subEntityName, subEntityId, rootEntityName, rootEntityId, this.RequestUrl, IsAjaxRequest());
                return;

            }
            catch
            {
                await Task.CompletedTask;
                return;
            }
        }

        protected string GetExportedFileName(string formName, string agencyIdentifier)
        {
            return string.Format("{0} - {1} - {2}.{3}", formName, agencyIdentifier, DateTime.Now.ToString("yyyyMMddhhmmtt", System.Globalization.CultureInfo.InvariantCulture), "pdf");
        }

        protected string GetErrors()
        {
            return string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
        }
    }
}
