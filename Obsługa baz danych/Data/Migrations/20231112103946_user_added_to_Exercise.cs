﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obsługa_baz_danych.Data.Migrations
{
    public partial class user_added_to_Exercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Exercise",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_UserId",
                table: "Exercise",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_AspNetUsers_UserId",
                table: "Exercise",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_AspNetUsers_UserId",
                table: "Exercise");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_UserId",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exercise");
        }
    }
}
