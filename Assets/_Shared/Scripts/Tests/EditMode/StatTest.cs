using NUnit.Framework;

namespace Enginooby.Tests {
  public class StatTest {
    // A Test behaves as an ordinary method
    [Test]
    public void AddStat() {
      // Assign
      var stat = new Stat("Test");

      // Act
      stat.Add(1);

      // Asset
      Assert.AreEqual(stat.CurrentValue, 1);
    }

    [Test]
    public void SetStatNotClamped() {
      // Assign
      var stat = new Stat("Test");

      // Act
      stat.Set(1);

      // Assert
      Assert.AreEqual(stat.CurrentValue, 1);
    }

    [Test]
    public void SetStatMinClamped() {
      // Assign
      var stat = new Stat("Test") {
        EnableMin = true,
        MinValue = 2,
      };

      // Act
      stat.Set(1);

      // Assert
      Assert.AreEqual(stat.CurrentValue, 2);
    }

    [Test]
    public void SetStatMaxClamped() {
      // Assign
      var stat = new Stat("Test") {
        EnableMax = true,
        MaxValue = 0,
      };

      // Act
      stat.Set(1);

      // Assert
      Assert.AreEqual(stat.CurrentValue, 0);
    }

    [Test]
    public void GetCurrentPercentage() {
      // Assign
      var stat = new Stat("Test") {
        EnableMax = true,
        MaxValue = 50,
      };

      // Act
      stat.Set(10);

      // Assert
      Assert.AreEqual(stat.CurrentPercentage, 20f);
    }
  }
}