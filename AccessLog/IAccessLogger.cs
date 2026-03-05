using System.Threading.Tasks;

namespace GenericFunctions.AccessLog
{
    public interface IAccessLogger
    {
        Task SaveAsync<TEntity>(TEntity log) where TEntity : LogData;
        Task SaveAsync(string actionName, string entityName, int entityId, string subEntityName, int subEntityId, string requestUrl = null);
        Task SaveAsync(string actionName, string entityName, int entityId, string subEntityName, int subEntityId, string rootEntityName, int rootEntityId, string requestUrl, bool isAjaxRequest = false);
    }
}
