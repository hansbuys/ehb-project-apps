using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsAdminRole");

                    b.Property<string>("Name");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AskNewPasswordOnNextLogin");

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model.Role", b =>
                {
                    b.HasOne("Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });
        }
    }
}
