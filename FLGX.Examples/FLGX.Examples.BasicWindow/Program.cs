/*
    Updated for FLGX v2.1.0 #
 */


namespace flgx.Examples.BasicWindow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FLGX Examples: 01 - Basic Window");

            FLGX.Init(new FLGXInitSettings(RenderingAPI.OpenGL));

            var window = FLGX.CreateWindow("Hello, FLGX!", 1600, 900);

            FLGX.MakeWindowCurrent(window);

            FLGX.ClearColor(new System.Numerics.Vector4(0.2f, 0.1f, 0.3f, 1.0f));
            window.Run(
                (float dt) =>
                {
                    FLGX.NewFrame();

                    FLGX.EndFrame();
                }
            );

            FLGX.Shutdown();
        }
    }
}
