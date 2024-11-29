using Serilog;
using Tasky.Administration.EntityFrameworkCore;
using Tasky.Projects.EntityFrameworkCore;
using Tasky.SaaS.EntityFrameworkCore;
using Tasky.WebApp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace Tasky.DbMigrator;

internal class Program
{
    private static async Task Main(string[] args)
    {
        TaskyLogging.Initialize();

        var builder = Host.CreateApplicationBuilder(args);

        builder.AddServiceDefaults();

        builder.AddNpgsqlDbContext<AdministrationDbContext>(connectionName: TaskyNames.AdministrationDb);
        builder.AddNpgsqlDbContext<IdentityDbContext>(connectionName: TaskyNames.IdentityServiceDb);
        builder.AddNpgsqlDbContext<SaaSDbContext>(connectionName: TaskyNames.SaaSDb);
        builder.AddNpgsqlDbContext<ProjectsDbContext>(connectionName: TaskyNames.ProjectsDb);
        builder.AddNpgsqlDbContext<WebAppDbContext>(connectionName: TaskyNames.WebAppDb);

        builder.Configuration.AddAppSettingsSecretsJson();

        builder.Logging.AddSerilog();

        builder.Services.AddHostedService<DbMigratorHostedService>();

        var host = builder.Build();

        await host.RunAsync();
    }
}
