using ButterMove;

namespace ButterMoveTest;

public class Tests
{
    private AmountCalculator _amountService;

    [SetUp]
    public void Setup()
    {
        _amountService = new AmountCalculator();
    }

    [Test]
    public void TestUnsupportedState()
    {
        //Assert.Throws<Exception>(_amountService.GetAmount());
        Assert.That(() => _amountService.GetAmount(), Throws.TypeOf<Exception>());
        //.That(_amountService.GetAmount(), Is.EqualTo(2500m));
    }
}
