using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Core.Identity.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Clients = new Collection<Client>();
            UserPermissions = new Collection<UserPermission>();
        }

        public virtual ICollection<Client> Clients { get; set; }

        public virtual ICollection<UserPermission> UserPermissions { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string CurrentClientId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string CurrentClientKey { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            if (!string.IsNullOrEmpty(CurrentClientId))
            {
                userIdentity.AddClaim(new Claim("AspNet.Identity.ClientId", CurrentClientId));
            }
            if (!string.IsNullOrEmpty(CurrentClientKey))
            {
                userIdentity.AddClaim(new Claim("AspNet.Identity.ClientKey", CurrentClientKey));
            }
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }


    [System.ComponentModel.DataAnnotations.Schema.Table("AspNetClients")]
    public class Client
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        public string ClientKey { get; set; }

        public string UserId { get; set; }
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("AspNetPermissions")]
    public class Permission
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }

        public string Area { get; set; }
        
        public string Description { get; set; }
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("AspNetUserPermissions")]
    public class UserPermission
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string UserId { get; set; }
        [System.ComponentModel.DataAnnotations.Key]
        public string PermissionId { get; set; }
        public int Level { get; set; }

        public virtual Permission PermissionModel { get; set; }
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("AspNetRolePermissions")]
    public class RolePermission
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string RoleId { get; set; }
        [System.ComponentModel.DataAnnotations.Key]
        public string PermissionId { get; set; }
        public int Level { get; set; }

        public virtual Permission PermissionModel { get; set; }
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("MEMNavbar")]
    public class NavBar
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }
        
        public string Name { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Area { get; set; }

        public string ImageClass { get; set; }

        public string Category { get; set; }

        public string PermissionId { get; set; }

        public int Seq { get; set; }
    }


    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
            : base()
        {
            RolePermissions = new Collection<RolePermission>();
        }
        
        public ApplicationRole(string iRoleName)
            : base(iRoleName)
        {
        }
        
        public string Description { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()//注册一个实体类型为模型的一部分,返回的对象用于配置这个实体
.HasMany<Client>(p => p.Clients)//从这个实体类型配置一个多关系
.WithOptional().HasForeignKey<string>(fk => fk.UserId).WillCascadeOnDelete(true);

            modelBuilder.Entity<UserPermission>().HasKey(p => new { p.UserId, p.PermissionId })
                .HasRequired(e => e.PermissionModel);
            modelBuilder.Entity<RolePermission>().HasKey(p => new { p.RoleId, p.PermissionId })
                .HasRequired(e => e.PermissionModel);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<UserPermission>(u => u.UserPermissions)
                .WithOptional().HasForeignKey<string>(fk => fk.UserId).WillCascadeOnDelete(true);
            modelBuilder.Entity<ApplicationRole>()
                .HasMany<RolePermission>(r => r.RolePermissions)
                .WithOptional().HasForeignKey<string>(fk => fk.RoleId).WillCascadeOnDelete(true);

            //modelBuilder.Entity<Permission>().ToTable("AspNetPermissions");
        }

        static ApplicationDbContext()
        {
            //当DB模型改变的时候请开启
            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        //Unity不需要此方法
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<NavBar> NavBars { get; set; }
    }
}