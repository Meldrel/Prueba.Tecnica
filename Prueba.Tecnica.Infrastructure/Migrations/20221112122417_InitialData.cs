using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prueba.Tecnica.Infrastructure.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationTime", "UserName", "Name", "Password", "Role" },
                values: new object[] { Guid.NewGuid(), DateTime.UtcNow, "Admin", "Admin", "MTIzUXdlcnQ=", "Administrador" }
                );
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationTime", "UserName", "Name", "Password", "Role" },
                values: new object[] { Guid.NewGuid(), DateTime.UtcNow, "User", "User", "MTIzUXdlcnQ=", "Usuario" }
                );

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "CreationTime", "Name", "ExpirationDate", "Type", },
                values: new object[] { Guid.NewGuid(), DateTime.UtcNow.AddDays(-5), "Item1", DateTime.UtcNow.AddDays(-3), "Type1" }
                );
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "CreationTime", "Name", "ExpirationDate", "Type", },
                values: new object[] { Guid.NewGuid(), DateTime.UtcNow.AddDays(-5), "Item2", DateTime.UtcNow.AddDays(5), "Type1" }
                );
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "CreationTime", "Name", "ExpirationDate", "Type", },
                values: new object[] { Guid.NewGuid(), DateTime.UtcNow.AddDays(-3), "Item3", DateTime.UtcNow.AddDays(-1), "Type2" }
                );
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "CreationTime", "Name", "ExpirationDate", "Type", },
                values: new object[] { Guid.NewGuid(), DateTime.UtcNow, "Item4", DateTime.UtcNow.AddDays(5), "Type3" }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
