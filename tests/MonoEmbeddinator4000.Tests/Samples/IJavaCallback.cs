using Android.Runtime;
using Java.Interop;

namespace MonoEmbeddinator4000.Tests
{
    [Register("mono.embeddinator.android.IJavaCallback", DoNotGenerateAcw = true)]
    public interface IJavaCallback : IJavaObject
    {
        [Export("send"), Register("send", "(Ljava/lang/String;)V", "MonoEmbeddinator4000.Tests.IJavaCallbackInvoker")]
        void Send(string text);
    }
}
