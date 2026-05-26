using Domain.Domains;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;
using FluentAssertions;

namespace Tests;


public class TransactionTests
{
    private readonly string _validMerchantId = "merchantid_132132";
    private readonly string _validCurrency = "AZN";
    private readonly string _validDescription = "Order #10492";

    [Fact]
    public void Create_WithValidParameters_ShouldInitializeTransactionCorrectly()
    {
        long amount = 10000;

        var transaction = Transaction.Create(_validMerchantId, Guid.NewGuid().ToString(), amount, _validCurrency, _validDescription, PaymentType.CARD, new string('0', 16), Guid.NewGuid().ToString());

        transaction.Should().NotBeNull();
        transaction.Id.Should().NotBeEmpty();
        transaction.MerchantId.Should().Be(_validMerchantId);
        transaction.Amount.Should().Be(amount);
        transaction.Currency.Should().Be(_validCurrency);
        transaction.Status.Should().Be(TransactionStatus.PENDING);
        transaction.Description.Should().Be(_validDescription);
    }

    [Theory]
    [InlineData(100, 3)]
    [InlineData(1000, 30)]
    [InlineData(50, 2)]
    [InlineData(115, 3)]
    [InlineData(117, 4)]
    public void Create_ShouldCalculateFeeUsingSymmetricRounding(long amount, long expectedFee)
    {
        // Act
        var transaction = Transaction.Create(_validMerchantId, Guid.NewGuid().ToString(), amount, _validCurrency, _validDescription, PaymentType.CARD, new string('0', 16), Guid.NewGuid().ToString());

        // Assert
        transaction.FeeAmount.Should().Be(expectedFee);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Create_WithAmountLessThanOrEqualToZero_ShouldThrowInvalidTransactionAmountException(long invalidAmount)
    {
        // Act
        Action act = () => Transaction.Create(_validMerchantId, Guid.NewGuid().ToString(), invalidAmount, _validCurrency, _validDescription, PaymentType.CARD, new string('0', 16), Guid.NewGuid().ToString());

        // Assert
        act.Should().Throw<InvalidTransactionAmountException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_WithNullOrEmptyCurrency_ShouldThrowException(string? invalidCurrency)
    {
        // Act
        Action act = () => Transaction.Create(_validMerchantId, Guid.NewGuid().ToString(), 1000, invalidCurrency!, _validDescription, PaymentType.CARD, new string('0', 16), Guid.NewGuid().ToString());

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Authorize_WhenStatusIsPending_ShouldTransitionToAuthorized()
    {
        // Arrange
        var transaction = Transaction.Create(_validMerchantId, Guid.NewGuid().ToString(), 5000, _validCurrency, _validDescription, PaymentType.CARD, new string('0', 16), Guid.NewGuid().ToString());

        // Act
        transaction.Authorize();

        // Assert
        transaction.Status.Should().Be(TransactionStatus.AUTHORIZED);
    }

    [Fact]
    public void Authorize_WhenStatusIsAuthorized_ShouldThrowInvalidDomainStateException()
    {
        // Arrange
        var transaction = Transaction.Create(
            _validMerchantId,
            Guid.NewGuid().ToString(),
            5000,
            _validCurrency,
            _validDescription,
            PaymentType.CARD,
            new string('0', 16),
            Guid.NewGuid().ToString());

        transaction.Authorize();

        // Act
        Action act = () => transaction.Authorize();

        // Assert
        act.Should()
            .Throw<InvalidDomainStateException>();
    }

    [Fact]
    public void Authorize_WhenStatusIsCaptured_ShouldThrowInvalidDomainStateException()
    {
        // Arrange
        var transaction = Transaction.Create(
            _validMerchantId,
            Guid.NewGuid().ToString(),
            5000,
            _validCurrency,
            _validDescription,
            PaymentType.CARD,
            new string('0', 16),
            Guid.NewGuid().ToString());

        transaction.Capture();

        // Act
        Action act = () => transaction.Authorize();

        // Assert
        act.Should()
            .Throw<InvalidDomainStateException>();
    }

    [Fact]
    public void Authorize_WhenStatusIsFailed_ShouldThrowInvalidDomainStateException()
    {
        // Arrange
        var transaction = Transaction.Create(
            _validMerchantId,
            Guid.NewGuid().ToString(),
            5000,
            _validCurrency,
            _validDescription,
            PaymentType.CARD,
            new string('0', 16),
            Guid.NewGuid().ToString());

        transaction.TransactionFailed();

        // Act
        Action act = () => transaction.Authorize();

        // Assert
        act.Should()
            .Throw<InvalidDomainStateException>();
    }

    [Fact]
    public void Authorize_WhenStatusIsVoided_ShouldThrowInvalidDomainStateException()
    {
        // Arrange
        var transaction = Transaction.Create(
            _validMerchantId,
            Guid.NewGuid().ToString(),
            5000,
            _validCurrency,
            _validDescription,
            PaymentType.CARD,
            new string('0', 16),
            Guid.NewGuid().ToString());

        transaction.TransactionVoided();

        // Act
        Action act = () => transaction.Authorize();

        // Assert
        act.Should()
            .Throw<InvalidDomainStateException>();
    }
}