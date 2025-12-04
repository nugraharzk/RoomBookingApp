using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class SchemaRevision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "Rooms");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "room");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "booking");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "user",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "user",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "user",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "user",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "user",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "user",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username",
                table: "user",
                newName: "ix_user_username");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "user",
                newName: "ix_user_email");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "room",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "room",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "room",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "room",
                newName: "capacity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "room",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "room",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "room",
                newName: "is_available");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "room",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "booking",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Purpose",
                table: "booking",
                newName: "purpose");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "booking",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "booking",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "booking",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "booking",
                newName: "start_time");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "booking",
                newName: "room_id");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "booking",
                newName: "end_time");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "booking",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserId",
                table: "booking",
                newName: "ix_booking_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_RoomId_StartTime_EndTime",
                table: "booking",
                newName: "ix_booking_room_id_start_time_end_time");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_room",
                table: "room",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_booking",
                table: "booking",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_booking_room_room_id",
                table: "booking",
                column: "room_id",
                principalTable: "room",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_booking_user_user_id",
                table: "booking",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_booking_room_room_id",
                table: "booking");

            migrationBuilder.DropForeignKey(
                name: "fk_booking_user_user_id",
                table: "booking");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_room",
                table: "room");

            migrationBuilder.DropPrimaryKey(
                name: "pk_booking",
                table: "booking");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "booking",
                newName: "Bookings");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_user_username",
                table: "Users",
                newName: "IX_Users_Username");

            migrationBuilder.RenameIndex(
                name: "ix_user_email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Rooms",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "Rooms",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Rooms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "capacity",
                table: "Rooms",
                newName: "Capacity");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Rooms",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Rooms",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_available",
                table: "Rooms",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Rooms",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Bookings",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "purpose",
                table: "Bookings",
                newName: "Purpose");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Bookings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Bookings",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Bookings",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "Bookings",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "room_id",
                table: "Bookings",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "end_time",
                table: "Bookings",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Bookings",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_booking_user_id",
                table: "Bookings",
                newName: "IX_Bookings_UserId");

            migrationBuilder.RenameIndex(
                name: "ix_booking_room_id_start_time_end_time",
                table: "Bookings",
                newName: "IX_Bookings_RoomId_StartTime_EndTime");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerHour",
                table: "Rooms",
                type: "numeric(10,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
