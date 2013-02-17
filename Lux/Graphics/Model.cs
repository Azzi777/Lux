using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lux.Framework;

namespace Lux.Graphics
{
    internal class Model
    {
        Mesh[] Meshes;

        static public Model UnitCube
        {
            get
            {
                Model m = new Model();
                m.Meshes = new Mesh[] { Mesh.UnitCube };

                return m;
            }
        }

        static public Model UnitIcosahedron
        {
            get
            {
                Model m = new Model();
                m.Meshes = new Mesh[] { Mesh.UnitIcosahedron };

                return m;
            }
        }

        static public Model LoadFromFile(string path)
        {
            Model m = new Model();
            byte[] fileData = File.ReadAllBytes(path);

            List<Mesh> meshes = new List<Mesh>();


            Mesh testMesh = Mesh.UnitCube;


            m.Meshes = meshes.ToArray();
            return m;
        }

        //public void Setup()
        //{
        //    foreach (Mesh m in Meshes)
        //    {
        //        m.Setup();
        //    }
        //}

        public void Render(Entity entity)
        {
            OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
            OpenTK.Graphics.OpenGL.GL.MultMatrix(entity.TransformMatrix.Data);
            foreach (Mesh m in Meshes)
            {
                m.Render();
            }
        }
    }
}
