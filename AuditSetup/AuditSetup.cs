using Audit.Core;
using Audit.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace AuditLog.AuditSetup
{
    /// <summary>
    /// Audit.NET setup
    /// </summary>
    public static class AuditSetup
    {
        /// <summary>
        /// Add the global audit filter to the MVC pipeline
        /// </summary>
        public static MvcOptions AuditSetupFilter(this MvcOptions mvcOptions)
        {
            mvcOptions.AddAuditFilter(audit => audit
                        .LogAllActions()
                        .WithEventType("{verb}.{controller}.{action}")
                        .IncludeModelState()
                        .IncludeRequestBody()
                        .IncludeResponseBody(ctx => ctx.HttpContext.Response.StatusCode == 200));

            return mvcOptions;
        }

        /// <summary>
        /// Add the audit middleware to the pipeline
        /// </summary>
        public static void AuditSetupMiddleware(this IApplicationBuilder app)
        {
            app.UseAuditMiddleware(audit => audit
                .FilterByRequest(r => !r.Path.Value!.EndsWith("favicon.ico"))
                .IncludeHeaders()
                .IncludeRequestBody()
                .IncludeResponseBody());
        }


        /// <summary>
        /// Setups the audit output
        /// </summary>
        public static void AuditSetupOutput(this WebApplication app)
        {
            Audit.Core.Configuration.JsonSettings.WriteIndented = true;

            // Include the trace identifier in the audit events
            var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
            Audit.Core.Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
            {
                scope.SetCustomField("TraceId", httpContextAccessor.HttpContext?.TraceIdentifier);
            });

            var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

            Audit.Core.Configuration
                .Setup()
                .UseSqlServer(config => config
                .ConnectionString(connectionString)
                .Schema("dbo")
                .TableName("AuditLog")
                .IdColumnName("EventId")
                .JsonColumnName("JsonData")
                .LastUpdatedColumnName("LastUpdatedDate")
                .CustomColumn("EventType", ev => ev.EventType)
                .CustomColumn("User", ev => ev.Environment.UserName));
        }
    }
}
