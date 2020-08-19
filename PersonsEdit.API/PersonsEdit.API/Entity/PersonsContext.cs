using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace PersonsEdit.API.Entity
{
    public partial class PersonsContext : DbContext
    {
        public PersonsContext()
        {
            // добавляем строки для первоначальной заполненности

            if (Persons.Count() != 0)
            {
                return;
            }
            Persons.AddRange(
                new Person()
                {
                    Login = "Misha1996",
                    Password = "MyDogIsCool",
                    Name = "Михаил Борисович",
                    Email = "miha1996@rambler.org"
                },
                new Person()
                {
                    Login = "Borian",
                    Password = "IamCOOL1982",
                    Name = "Борис Анатольевич",
                    Email = "borisbritva@mail.ru"
                },
                new Person()
                {
                    Login = "bamby2007",
                    Password = "qwerasdf1qaz",
                    Name = "Наташа",
                    Email = "natali@gmail.com"
                },
                new Person()
                {
                    Login = "admin",
                    Password = "admin",
                    Name = "admin",
                    Email = "admin@admin.admin"
                });

            Roles.AddRange(
                new Role()
                {
                    Name = "Администратор"
                },
                new Role()
                {
                    Name = "Заказчик"
                },
                new Role()
                {
                    Name = "Специалист отдела кадров"
                },
                new Role()
                {
                    Name = "Бухгалтер"
                });

            SaveChanges();

            PersonsRoles.AddRange(
                new PersonsRoles()
                {
                    PersonId = 1,
                    RoleId = 1,
                },
                new PersonsRoles()
                {
                    PersonId = 2,
                    RoleId = 2,
                },
                new PersonsRoles()
                {
                    PersonId = 3,
                    RoleId = 3,
                },
                new PersonsRoles()
                {
                    PersonId = 3,
                    RoleId = 4,
                },
                new PersonsRoles()
                {
                    PersonId = 4,
                    RoleId = 1,
                });

            SaveChanges();
        }

        public PersonsContext(DbContextOptions<PersonsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<PersonsRoles> PersonsRoles { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build()
                    ["PersonsContext:connectionString"];
                connectionString = connectionString.Replace("|DataDirectory|", AppDomain.CurrentDomain.BaseDirectory + "App_Data");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PersonsRoles>(entity =>
            {
                entity.HasKey(d => new { d.PersonId, d.RoleId });

                entity.ToTable("Persons_Roles");

                entity.HasOne(d => d.Person)
                    .WithMany()
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Persons_Roles_Persons");

                entity.HasOne(d => d.Role)
                    .WithMany()
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Persons_Roles_Roles");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
