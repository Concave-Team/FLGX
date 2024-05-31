using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Silk.NET.Direct3D.Compilers;
using Silk.NET.Core.Native;

namespace flgx.Internal
{
    public class FLGXInternalState
    {
        private Dictionary<string, object> StateVariables = new Dictionary<string, object>();
        public RenderingAPI RenderingAPI { get; set; }

        public DXGI dxgi = null!;
        public D3D11 d3d11 = null!;
        public D3DCompiler compiler = null!;

        public ComPtr<IDXGIFactory2> factory = default;
        public ComPtr<IDXGISwapChain1> swapchain = default;
        public ComPtr<ID3D11Device> device = default;
        public ComPtr<ID3D11DeviceContext> deviceContext = default;
        public ComPtr<ID3D11InputLayout> inputLayout = default;
        public ComPtr<ID3D11RenderTargetView> renderTarget = default;

        public object GetStateVariable(string stateId)
        {
            if (StateVariables.ContainsKey(stateId))
                return StateVariables[stateId];
            else
                throw new FLGXInternalStateException(this, "Couldn't access state variable at identifier "+stateId + " (it does not exist)");
        }

        public int GetStateVariableCount()
        {
            return StateVariables.Count;
        }

        public void SetStateVariable(string stateId, object value)
        {
            if (StateVariables.ContainsKey(stateId))
                StateVariables[stateId] = value;
            else
                throw new FLGXInternalStateException(this, "Couldn't access state variable at identifier " + stateId + " (it does not exist)");
        }

        public void CreateStateVariable(string stateId, object value)
        {
            if (!StateVariables.ContainsKey(stateId))
            {
                StateVariables.Add(stateId, value);
            }
            else
            {
                throw new FLGXInternalStateException(this, "Attempted to create a state variable that already exists: " + stateId);
            }
        }

        public string ListState()
        {
            string state = "";

            foreach (var stateVar in StateVariables)
                state += stateVar.Key + ": " + stateVar.Value.ToString()+"\n";

            return state;
        }
    }

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class FLGXInternalStateException : Exception 
    {
        /// <summary>
        /// The FLGX internal state during the moment of exception/crash.
        /// </summary>
        public FLGXInternalState? FaultyState;

        public FLGXInternalStateException(FLGXInternalState state, string message) : base(message)
        {
        }

        private string GetDebuggerDisplay()
        {
            return ToString()+"\nSTATE:\n"+FaultyState.ListState();
        }
    }
}
