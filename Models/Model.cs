using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class PermitContext : DbContext
{
    public DbSet<PermitSubmitter> PermitSubmitters { get; set; }
    public DbSet<PermitType> PermitTypes { get; set; }
    public DbSet<PermitApplicationInfo> permitApplicationInfos { get; set; }
    public string DbPath { get; }
    public PermitContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "permits.db");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite($"Data Source={DbPath}");
}

public class PermitSubmitter()
{
    [Key]
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int? PermitType { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? County { get; set; }
    public string? State { get; set; }
}

public class PermitType
{
    public int Id { get; set; }
    public string? Description { get; set; }
}

public class PermitApplicationInfo
{
    [Key]
    public int Application_id { get; set; }
    public int PermitType { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string? County { get; set; }
}
