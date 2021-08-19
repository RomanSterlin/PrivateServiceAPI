using Microsoft.JSInterop;

namespace BadooAPI.Utills
{
    public class JsEngine
    {
        public IJSRuntime JSRuntime { get; }

        //public IJSRuntime JSRuntime { get; private set; }

        public JsEngine(IJSRuntime Runtime)
        {
            JSRuntime = Runtime;
        }
        public async void Add()
        {
            var result = await JSRuntime.InvokeAsync<int>("add", 1, 2);
        }
    }
}
