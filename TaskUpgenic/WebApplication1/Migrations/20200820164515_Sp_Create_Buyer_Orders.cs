using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Sp_Create_Buyer_Orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Create procedure Sp_Create_Buyer_Orders 
                  @AspUserId nvarchar(450),
	@Seller_id nvarchar(450),
	@Price float
AS
BEGIN
	INSERT INTO [dbo].[Buyers]
           (
           [AspUserId]
           ,[Seller_id]
           ,[OrderDate]
           ,[Price])
     VALUES
           (
           @AspUserId,@Seller_id,GETDATE(),@Price);
END
";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Drop procedure Sp_Create_Buyer_Orders";
            migrationBuilder.Sql(procedure);
        }
    }
}
