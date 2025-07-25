﻿using BlogAs.Data.Mappings;
using BlogAs.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAs.Data;

public class BlogDataContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
 
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }

    public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryMap());
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new PostMap());
        modelBuilder.ApplyConfiguration(new TagMap());
    }
}