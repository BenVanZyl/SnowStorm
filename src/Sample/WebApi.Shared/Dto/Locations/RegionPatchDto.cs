using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared.Dto.Locations
{
    public class RegionPatchDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        public FieldNames FieldName => (FieldNames)Enum.Parse(typeof(FieldNames), Name, true);

        public enum FieldNames
        {
            Id,
            RegionDescription
        };
    }
}
