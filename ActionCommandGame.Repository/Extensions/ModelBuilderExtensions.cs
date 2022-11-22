using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ActionCommandGame.Repository.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder builder)
        {
            foreach (IMutableEntityType entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }
        }
    }
}
