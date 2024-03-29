﻿using Gamelib.DataStructures;
using NUnit.Framework;

public class TestCircularBuffer
{
    [Test]
    public void TestOverwrite()
    {
        var buffer = new CircularBuffer<long>(3);
        Assert.AreEqual(0, buffer.Count);
        Assert.AreEqual(default(long), buffer.Enqueue(1));
        Assert.AreEqual(1, buffer.Count);
        Assert.AreEqual(default(long), buffer.Enqueue(2));
        Assert.AreEqual(default(long), buffer.Enqueue(3));
        Assert.AreEqual(1, buffer.Enqueue(4));
        Assert.AreEqual(3, buffer.Count);
        Assert.AreEqual(2, buffer.Dequeue());
        Assert.AreEqual(3, buffer.Dequeue());
        Assert.AreEqual(4, buffer.Dequeue());
        Assert.AreEqual(0, buffer.Count);
    }

    [Test]
    public void TestUnderwrite()
    {
        var buffer = new CircularBuffer<long>(5);
        Assert.AreEqual(default(long), buffer.Enqueue(1));
        Assert.AreEqual(default(long), buffer.Enqueue(2));
        Assert.AreEqual(default(long), buffer.Enqueue(3));
        Assert.AreEqual(3, buffer.Count);
        Assert.AreEqual(1, buffer.Dequeue());
        Assert.AreEqual(2, buffer.Dequeue());
        Assert.AreEqual(3, buffer.Dequeue());
        Assert.AreEqual(0, buffer.Count);
    }

    [Test]
    public void TestIncreaseCapacityWhenFull()
    {
        var buffer = new CircularBuffer<long>(3);
        Assert.AreEqual(default(long), buffer.Enqueue(1));
        Assert.AreEqual(default(long), buffer.Enqueue(2));
        Assert.AreEqual(default(long), buffer.Enqueue(3));
        Assert.AreEqual(3, buffer.Count);
        buffer.Capacity = 4;
        Assert.AreEqual(3, buffer.Count);
        Assert.AreEqual(1, buffer.Dequeue());
        Assert.AreEqual(2, buffer.Dequeue());
        Assert.AreEqual(3, buffer.Dequeue());
        Assert.AreEqual(0, buffer.Count);
    }

    [Test]
    public void TestDecreaseCapacityWhenFull()
    {
        var buffer = new CircularBuffer<long>(3);
        Assert.AreEqual(default(long), buffer.Enqueue(1));
        Assert.AreEqual(default(long), buffer.Enqueue(2));
        Assert.AreEqual(default(long), buffer.Enqueue(3));
        Assert.AreEqual(3, buffer.Count);
        buffer.Capacity = 2;
        Assert.AreEqual(2, buffer.Count);
        Assert.AreEqual(1, buffer.Dequeue());
        Assert.AreEqual(2, buffer.Dequeue());
        Assert.AreEqual(0, buffer.Count);
    }

    [Test]
    public void TestEnumerationWhenFull()
    {
        var buffer = new CircularBuffer<long>(3);
        Assert.AreEqual(default(long), buffer.Enqueue(1));
        Assert.AreEqual(default(long), buffer.Enqueue(2));
        Assert.AreEqual(default(long), buffer.Enqueue(3));
        var i = 0;
        foreach (var value in buffer)
            Assert.AreEqual(++i, value);
        Assert.AreEqual(i, 3);
    }

    [Test]
    public void TestEnumerationWhenPartiallyFull()
    {
        var buffer = new CircularBuffer<long>(3);
        Assert.AreEqual(default(long), buffer.Enqueue(1));
        Assert.AreEqual(default(long), buffer.Enqueue(2));
        var i = 0;
        foreach (var value in buffer)
            Assert.AreEqual(++i, value);
        Assert.AreEqual(i, 2);
    }

    [Test]
    public void TestEnumerationWhenEmpty()
    {
        var buffer = new CircularBuffer<long>(3);
        foreach (var value in buffer)
            Assert.IsTrue(false);
    }

    [Test]
    public void TestRemoveAt()
    {
        var buffer = new CircularBuffer<long>(5);
        Assert.AreEqual(default(long), buffer.Enqueue(1));
        Assert.AreEqual(default(long), buffer.Enqueue(2));
        Assert.AreEqual(default(long), buffer.Enqueue(3));
        Assert.AreEqual(default(long), buffer.Enqueue(4));
        Assert.AreEqual(default(long), buffer.Enqueue(5));
        buffer.RemoveAt(buffer.IndexOf(2));
        buffer.RemoveAt(buffer.IndexOf(4));
        Assert.AreEqual(3, buffer.Count);
        Assert.AreEqual(1, buffer.Dequeue());
        Assert.AreEqual(3, buffer.Dequeue());
        Assert.AreEqual(5, buffer.Dequeue());
        Assert.AreEqual(0, buffer.Count);
        Assert.AreEqual(default(long), buffer.Enqueue(1));
        Assert.AreEqual(default(long), buffer.Enqueue(2));
        Assert.AreEqual(default(long), buffer.Enqueue(3));
        Assert.AreEqual(default(long), buffer.Enqueue(4));
        Assert.AreEqual(default(long), buffer.Enqueue(5));
        buffer.RemoveAt(buffer.IndexOf(1));
        buffer.RemoveAt(buffer.IndexOf(3));
        buffer.RemoveAt(buffer.IndexOf(5));
        Assert.AreEqual(2, buffer.Count);
        Assert.AreEqual(2, buffer.Dequeue());
        Assert.AreEqual(4, buffer.Dequeue());
        Assert.AreEqual(0, buffer.Count);
    }
}
