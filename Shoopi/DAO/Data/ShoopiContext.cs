using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAO.Data;

public partial class ShoopiContext : DbContext
{
    public ShoopiContext()
    {
    }

    public ShoopiContext(DbContextOptions<ShoopiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__Favorite__749FA5A7C6BE2810");

            entity.ToTable("Favorite");

            entity.Property(e => e.FavoriteId).HasColumnName("Favorite_ID");
            entity.Property(e => e.Detail).HasMaxLength(255);
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.Product).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Favorite__Produc__59FA5E80");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Favorite__User_I__5812160E");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__F1E4639BE27300A0");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Delivery).HasMaxLength(50);
            entity.Property(e => e.FeeDelivery).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.MethodPayment).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatus_ID");
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .HasConstraintName("FK__Order__OrderStat__59063A47");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__User_ID__59FA5E80");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__53D880E0D6F55459");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetail_ID");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__5AEE82B9");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__Produ__5EBF139D");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("PK__OrderSta__489D9CD2AD3C5E21");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatus_ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__9834FB9A262F7879");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.AliasName).HasMaxLength(50);
            entity.Property(e => e.Detail).HasMaxLength(255);
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Mfg)
                .HasMaxLength(255)
                .HasColumnName("MFG");
            entity.Property(e => e.Picture).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.TypeId).HasColumnName("Type_ID");

            entity.HasOne(d => d.Type).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Product__Type_ID__5FB337D6");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__D80AB49B663AFDBD");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Type__FE90DDFE57D0C5BE");

            entity.ToTable("Type");

            entity.Property(e => e.TypeId).HasColumnName("Type_ID");
            entity.Property(e => e.Detail).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__206D9190A839353B");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("User_ID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Picture).HasMaxLength(255);
            entity.Property(e => e.RandomKey)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");

            // Add mappings for IsGoogleAccount and IsFacebookAccount
            entity.Property(e => e.IsGoogleAccount).HasColumnName("IsGoogleAccount");
            entity.Property(e => e.IsFacebookAccount).HasColumnName("IsFacebookAccount");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict) // Adjust as needed
                .HasConstraintName("FK__User__Role_ID__5DCAEF64");
        });


        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.UserLoginId).HasName("PK__UserLogi__EF923B588508FBE3");

            entity.ToTable("UserLogin");

            entity.Property(e => e.UserLoginId).HasColumnName("UserLogin_ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LoginProvider).HasMaxLength(50);
            entity.Property(e => e.ProviderKey).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLogin_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
