using PeikkoPrecastWallDesigner.Domain.Entities;
using PeikkoPrecastWallDesigner.Domain.Enums;
using PeikkoPrecastWallDesigner.Domain.Exceptions;

namespace PeikkoPrecastWallDesigner.Domain.Services.Computations
{
	/// <summary>
	/// Computing Domain Service
	/// </summary>
	public partial class ComputingDomainService : IComputingDomainService
	{
		public void GeometryValidation(Layers data)
		{
			if (!CenterGravityValidation(data))
				throw new GeometryValidationException(
					"Centre of gravity outside of internal layer",
					EGeometryValidationExceptionType.CenterGravityOutOfBounds);
			if (!InsulatedLayerValidation(data))
				throw new GeometryValidationException(
					"Invalid thickness of insulation",
					EGeometryValidationExceptionType.InsulationThicknessInvalid);
			if (!HolePositionValidation(data))
				throw new GeometryValidationException(
					"Invalid hole position",
					EGeometryValidationExceptionType.HolePositionInvalid);
		}
		public List<LayerLoads> ComputeLoads(Layers data)
		{
			double overlap;
			LayerLoads internalLoads;
			LayerLoads externalLoads;
   
                        //----------------------------
			// Old single hole logic (Cathy added)
   			//----------------------------
                        /*
			if (data.Hole.Position == EHolePosition.Internal)
			{
				overlap = HoleOverlap(data.Hole, data.InternalLayer);
				internalLoads = ComputeLayerLoads(data.InternalLayer, overlap);
				externalLoads = ComputeLayerLoads(data.ExternalLayer, 0);
			}
			else if (data.Hole.Position == EHolePosition.External)
			{
				overlap = HoleOverlap(data.Hole, data.ExternalLayer);
				internalLoads = ComputeLayerLoads(data.InternalLayer, 0);
				externalLoads = ComputeLayerLoads(data.ExternalLayer, overlap);
			}
			else
			{
				overlap = HoleOverlap(data.Hole, data.InternalLayer);
				internalLoads = ComputeLayerLoads(data.InternalLayer, overlap);
				overlap = HoleOverlap(data.Hole, data.ExternalLayer);
				externalLoads = ComputeLayerLoads(data.ExternalLayer, overlap);
			}
                        */
			//-------------------------------
                        // New mutiple hole logic 
			//-------------------------------
   
                        double internalOverlap = 0;
			double externalOverlap = 0;

   
                        foreach (var hole in data.Holes)
			{
				if (hole.Position == EHolePosition.Internal)
					internalOverlap += HoleOverlap(hole, data.InternalLayer);
				else if (hole.Position == EHolePosition.External)
					externalOverlap += HoleOverlap(hole, data.ExternalLayer);
				else
				{
					internalOverlap += HoleOverlap(hole, data.InternalLayer);
					externalOverlap += HoleOverlap(hole, data.ExternalLayer);
				}
			}

			internalLoads = ComputeLayerLoads(data.InternalLayer, internalOverlap);
			externalLoads = ComputeLayerLoads(data.ExternalLayer, externalOverlap);
   
			return [internalLoads, externalLoads];
		}
	}
}
