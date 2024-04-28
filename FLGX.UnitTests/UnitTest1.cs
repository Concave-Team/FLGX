using FLGX.Internal;

namespace FLGX.UnitTests
{
    public class Tests
    {
        FLGXInternalState state;
        [SetUp]
        public void Setup()
        {
            state = new FLGXInternalState();
        }

        [Test]
        public void Test_InternalState_Initialization()
        {
            Assert.IsNotNull(state);
            Assert.That(state.GetStateVariableCount(), Is.EqualTo(0));
        }

        [Test]
        public void Test_InternalState_AreValuesChanged()
        {
            state.CreateStateVariable("abcd", 15);

            Assert.DoesNotThrow(() => { state.GetStateVariable("abcd"); });

            Assert.IsNotNull(state.GetStateVariable("abcd"));

            Assert.That((int)state.GetStateVariable("abcd") == 15);

            state.SetStateVariable("abcd", 18);

            Assert.DoesNotThrow(() => { state.GetStateVariable("abcd"); });

            Assert.IsNotNull(state.GetStateVariable("abcd"));

            Assert.That((int)state.GetStateVariable("abcd") == 18);
        }

        [Test]
        public void Test_InternalState_ThrowsOnIncorrectData()
        {
            var ex1 = (FLGXInternalStateException)Assert.Throws(typeof(FLGXInternalStateException), () => { state.GetStateVariable("abcde"); });
            var ex2 = (FLGXInternalStateException)Assert.Throws(typeof(FLGXInternalStateException), () => { state.SetStateVariable("abcde", -1); });
        }
    }
}