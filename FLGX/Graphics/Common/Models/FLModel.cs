using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using flgx.Internal;
using Silk.NET.Vulkan;

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

        public void DrawInstances(int count)
        {
            foreach (FLMesh mesh in Meshes)
            {
                VtxStruct.Bind();
                mesh.DrawInstances(count);
            }
        }

        public void Destroy()
        {
            Meshes.Clear();
            VtxStruct.Destroy();
        }

        public static FLModel FromPrimitiveData(int[] indices, FLVertex[] vertices)
        {
            var model = new FLModel();
            model.VtxStruct = FLGX.CreateFLVertexStructure();
            model.VtxStruct.Bind();
            model.Meshes.Add(new FLMesh(indices, vertices));
            return model;
        }

        public static FLModel FromFile(string file)
        {
            var importer = new AssimpContext();
            var model = new FLModel();
            var scene = FLGX.InternalState.RenderingAPI == RenderingAPI.OpenGL ? importer.ImportFile(file, PostProcessSteps.Triangulate | PostProcessSteps.GenerateUVCoords | PostProcessSteps.GenerateNormals | PostProcessSteps.SortByPrimitiveType | PostProcessSteps.FlipUVs) : 
                importer.ImportFile(file, PostProcessSteps.Triangulate | PostProcessSteps.GenerateUVCoords | PostProcessSteps.MakeLeftHanded | PostProcessSteps.GenerateNormals | PostProcessSteps.SortByPrimitiveType);
            model.VtxStruct = FLGX.CreateFLVertexStructure();
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
                if (FLGX.InternalState.RenderingAPI == RenderingAPI.OpenGL)
                    FLGX.SetFLVertexAttributes(model.VtxStruct);
                model.Meshes.Add(_mesh);
            }

            return model;
        }
    }
}
