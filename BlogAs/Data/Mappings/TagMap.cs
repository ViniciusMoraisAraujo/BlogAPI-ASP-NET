using BlogAs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAs.Data.Mappings;

public class TagMap : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tag");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd().UseIdentityColumn();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Slug);

        builder.HasMany(x => x.Posts)
            .WithMany(x => x.Tags)
            .UsingEntity<Dictionary<string, object>>(
                "TagPost", 
                tag => tag
                    .HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("FK_TagPost_PostId")
                    .OnDelete(DeleteBehavior.Cascade),
                post => post.HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName("FK_TagPost_TagId")
                    .OnDelete(DeleteBehavior.Cascade)
                );
    }
}