using Domain.Domains;
using Domain.Shared.Enums;
using FluentAssertions;

namespace Tests;

public class PaymentSnapshotTests
{
    private readonly Guid _validTransactionId = Guid.NewGuid();

    [Fact]
    public void Create_WithValidCard_ShouldMaskCorrectlyAndKeepFirstFourDigits()
    {
        // Arrange
        var card = "4242111122223333";
        var paymentType = PaymentType.CARD;

        // Act
        var snapshot = PaymentSnapshot.Create(paymentType, card);

        // Assert
        snapshot.Should().NotBeNull();
        snapshot.Id.Should().NotBeEmpty();
        snapshot.Type.Should().Be(paymentType);
        snapshot.MaskedIdentifier.Should().Be("4242****");
    }

    [Fact]
    public void Create_WithShortCardNumber_ShouldFallbackToGenericTokenMasking()
    {
        // Arrange
        var shortCard = "12345";
        var paymentType = PaymentType.CARD;

        // Act
        var snapshot = PaymentSnapshot.Create(paymentType, shortCard);

        // Assert
        snapshot.MaskedIdentifier.Should().Be("***_2345");
    }

    [Fact]
    public void Create_WithValidBankTransfer_ShouldMaskMiddleAndKeepEnds()
    {
        // Arrange
        var iban = "AZ29PABA0133344455556666";
        var paymentType = PaymentType.BANK_TRANSFER;

        // Act
        var snapshot = PaymentSnapshot.Create(paymentType, iban);

        // Assert
        snapshot.MaskedIdentifier.Should().Be("AZ29********6666");
    }

    [Fact]
    public void Create_WithShortBankIdentifier_ShouldReturnFourStars()
    {
        // Arrange
        var shortIban = "AZ29";
        var paymentType = PaymentType.BANK_TRANSFER;

        // Act
        var snapshot = PaymentSnapshot.Create(paymentType, shortIban);

        // Assert
        snapshot.MaskedIdentifier.Should().Be("****");
    }

    [Fact]
    public void Create_WithFallbackOrUnknownPaymentType_ShouldUseGenericMasking()
    {
        // Arrange
        var rawToken = "secret_wallet_token_1234";
        var unknownType = (PaymentType)99;

        // Act
        var snapshot = PaymentSnapshot.Create(unknownType, rawToken);

        // Assert
        snapshot.MaskedIdentifier.Should().Be("***_1234");
    }

    [Fact]
    public void Create_WithShortGenericToken_ShouldReturnFourStars()
    {
        // Arrange
        var shortToken = "abc";
        var unknownType = (PaymentType)99;

        // Act
        var snapshot = PaymentSnapshot.Create(unknownType, shortToken);

        // Assert
        snapshot.MaskedIdentifier.Should().Be("****");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_WithNullOrEmptyIdentifier_ShouldThrowException(string? invalidIdentifier)
    {
        // Act
        Action act = () => PaymentSnapshot.Create(PaymentType.CARD, invalidIdentifier!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}