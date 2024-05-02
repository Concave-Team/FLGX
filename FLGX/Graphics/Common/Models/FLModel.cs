using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;

namespace flgx.Graphics.Common.Models
{
    public class FLModel
    {
        public VertexStructure VtxStruct;
        public List<FLMesh> Meshes = new List<FLMesh>();

        public void Draw()
        {
            foreach (FLMesh mesh in Meshes)
            {
                VtxStruct.Bind();
                mesh.Draw(); 
            }
        }

        public void Destroy()
        {
            Meshes.Clear();
            VtxStruct.Destroy();
        }

        public static FLModel FromFile(string file)
        {
            var importer = new AssimpContext();
            var model = new FLModel();
            var scene = importer.ImportFile(file, PostProcessSteps.Triangulate | PostProcessSteps.GenerateUVCoords | PostProcessSteps.GenerateNormals | PostProcessSteps.SortByPrimitiveType | PostProcessSteps.FlipUVs);

            model.VtxStruct = FLGX.CreateVertexStructure();
            foreach (var mesh in scene.Meshes)
            {
                var vertices = mesh.Vertices; // Positions
                var normals = mesh.Normals;   // Normals
                var texcoords = mesh.TextureCoordinateChannels[0]; // Texcoords for first UV channel

                var Vertices = new List<FLVertex>();

                for (int i = 0; i < mesh.VertexCount; i++)
                {
                    Vector3D vertex = vertices[i];
                    Vector3D normal = normals[i];
                    System.Numerics.Vector2 texcoord = new System.Numerics.Vector2(texcoords[i].X, texcoords[i].Y);

                    Vertices.Add(new FLVertex(vertex.ToSNV2(), normal.ToSNV2(), texcoord));
                }

                model.VtxStruct.Bind();
                FLMesh _mesh = new FLMesh(mesh.GetIndices(), Vertices.ToArray());
                FLGX.SetFLVertexAttributes(model.VtxStruct);
                model.Meshes.Add(_mesh);
            }

            return model;
        }
    }
}
