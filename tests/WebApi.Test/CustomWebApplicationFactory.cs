using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public InvoiceIdentityManager Invoice_Admin { get; private set; } = default!;
    public InvoiceIdentityManager Invoice_MemberTeam { get; private set; } = default!;
    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<BarberBossDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BarberBossDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncripter, accessTokenGenerator);
            });
    }

    private void StartDatabase(
        BarberBossDbContext dbContext,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncripter, accessTokenGenerator);
        var invoiceTeamMember = AddInvoicing(dbContext, userTeamMember, invoiceId: 1, tagId: 1);
        Invoice_MemberTeam = new InvoiceIdentityManager(invoiceTeamMember);

        var userAdmin = AddUserAdmin(dbContext, passwordEncripter, accessTokenGenerator);
        var invoiceAdmin = AddInvoicing(dbContext, userAdmin, invoiceId: 2, tagId: 2);
        Invoice_Admin = new InvoiceIdentityManager(invoiceAdmin);

        dbContext.SaveChanges();
    }

    private User AddUserTeamMember(
        BarberBossDbContext dbContext,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(
        BarberBossDbContext dbContext,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Invoice AddInvoicing(BarberBossDbContext dbContext, User user, long invoiceId, long tagId)
    {
        var invoice = InvoiceBuilder.Build(user);
        invoice.Id = invoiceId;

        foreach(var tag in invoice.Tags)
        {
            tag.Id = tagId;
            tag.InvoiceId = invoiceId;
        }

        dbContext.Invoicing.Add(invoice);

        return invoice;
    }
}
