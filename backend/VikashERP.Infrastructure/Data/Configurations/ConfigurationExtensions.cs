using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VikashERP.Infrastructure.Data.Configurations;

public static class ConfigurationExtensions
{
    public static void ConfigureGuidPrimaryKey<TEntity>(
        this EntityTypeBuilder<TEntity> entity,
        Expression<Func<TEntity, Guid>> keyExpression) where TEntity : class =>
        entity.Property(keyExpression).ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
}
