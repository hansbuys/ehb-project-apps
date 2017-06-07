using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20170606153729_Add_Users")]
    partial class Add_Users
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsAdminRole");

                    b.Property<string>("Name");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AskNewPasswordOnNextLogin");

                    b.Property<string>("Password");

                    b.Property<string>("Salt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Role", b =>
                {
                    b.HasOne("Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });
        }
    }
}
