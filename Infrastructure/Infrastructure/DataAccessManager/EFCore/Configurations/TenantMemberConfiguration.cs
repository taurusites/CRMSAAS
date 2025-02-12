using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class TenantMemberConfiguration : BaseEntityConfiguration<TenantMember>
{
    public override void Configure(EntityTypeBuilder<TenantMember> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.UserId).HasMaxLength(UserIdConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Name).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Description).HasMaxLength(DescriptionConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.TenantId).HasMaxLength(IdConsts.MaxLength).IsRequired(false);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Name);
    }
}

