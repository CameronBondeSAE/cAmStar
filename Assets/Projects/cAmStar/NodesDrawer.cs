using Shapes;
using UnityEngine;

namespace Shapes
{
	// I recommend drawing in onPreRender, which ImmediateModeShapeDrawer will handle for you, so let's inherit from that
	[ExecuteAlways]
	public class NodesDrawer : ImmediateModeShapeDrawer
	{
		public cAmStar camStar;
		public Map map;
		
		// This method is called by ImmediateModeShapeDrawer - equivalent to OnPreRender of all cameras (except preview windows and probe cameras)
		public override void DrawShapes(Camera cam)
		{
			// Draw.Command enqueues a set of draw commands to render in the given camera
			using (Draw.Command(cam))
			{
				// all immediate mode drawing should happen within these using-statements
				Draw.ResetAllDrawStates(); // this makes sure no static draw states "leak" over to this scene
				
				
				// Occlusions
				float maxOcclusionValue = map.numberOfAngles;
				
				for (int x = 0; x < map.size.x; x++)
				{
					for (int y = 0; y < map.size.y; y++)
					{
						if (map.grid != null)
						{
							if (map.grid[x, y].isBlocked)
							{
								// Gizmos.color = Color.red;
								// Gizmos.DrawCube(new Vector3(x, 0, y), Vector3.one);
							}
							else
							{
								// numberOfAngles
								// numberOfAngles* maxDistance;
				
								// CHECK: Slow to change colours so often
								// Scale the value to a 0 to 255 value for colours.
								float occlusionScore = 1f - (map.grid[x, y].occlusionScore / maxOcclusionValue);
								Draw.Color = new Color(occlusionScore, occlusionScore, occlusionScore, 1f-occlusionScore/2f);
								Draw.Cuboid(new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
							}
						}
					}
				}

				
				
				
				
				
								
				
				
				
				
				// Nodes
				
				Vector3 size = new Vector3(1, 0.1f, 1);
				
				if (camStar != null)
				{
					Draw.Color = new Color(1f, 0.9215686f, 0.01568628f, 0.5f);
					foreach (Node node in camStar.open)
					{
						Draw.Cuboid(new Vector3(node.position.x, 0, node.position.y), size);
					}
				
					Draw.Color = new Color(0, 0, 0.5f, 0.5f);
					foreach (Node node in camStar.closed)
					{
						Draw.Cuboid(new Vector3(node.position.x, 0, node.position.y), size);
					}
				
					Draw.Color = Color.green;
					foreach (Node node in camStar.finalPath)
					{
						Draw.Cuboid(new Vector3(node.position.x, 0, node.position.y), size);
					}
				}
			}
		}
	}
}