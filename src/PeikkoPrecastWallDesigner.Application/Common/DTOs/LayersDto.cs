namespace PeikkoPrecastWallDesigner.Application.Common.DTOs
{
	public record LayersDto
	{
		public LayerDto InternalLayer { get; set; }
		public LayerDto ExternalLayer { get; set; }
		public double InsulatedLayerThickness { get; set; }
		public List<HoleDto> Holes { get; set; } // Cathy changed from HoleDto to List<HoleDto>
	}
}
