using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        //configure the primary key
        builder.HasKey(x => x.Id);

        //configure the index
        builder.HasIndex(x => new { x.UserId, x.Token }).IsUnique();

        //configure the relationship
        builder
            .HasOne(token => token.User)
            .WithOne(user => user.RefreshToken)
            .HasForeignKey<RefreshToken>(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
