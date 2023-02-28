using System.Diagnostics;
using NUnit.Framework;

namespace ObjectActuality.Test;

public class TestActuality
{
    [Test]
    public void TestAddOrUpdateSingleItem()
    {
        var inst = new ObjectActuality.Actuality<string>();
        var item = "item";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var addOrUpdate2 = inst.TryAddOrUpdate(item, 2000);
        var time2 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.That(addOrUpdate2);

        Assert.That(time1 == 1000);
        Assert.That(time2 == 2000);
    }

    [Test]
    public void TestAddOrUpdateTwoItems()
    {
        var inst = new Actuality<string>();
        var item = "item";
        var item2 = "item2";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var addOrUpdate2 = inst.TryAddOrUpdate(item2, 2000);
        var time2 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.That(addOrUpdate2);

        Assert.That(time1 == 1000);
        Assert.That(time2 == 2000);
    }

    [Test]
    public void TestAddOrUpdateThenRemoveSingleItemEqualTime()
    {
        var inst = new Actuality<string>();
        var item = "item";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var noItemLeft = inst.PurgeItemsActualEnough(1000);
        var time2 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.That(noItemLeft);

        Assert.That(time1 == 1000);
        Assert.That(time2 == 0);
    }

    [Test]
    public void TestAddOrUpdateThenRemoveSingleItemGreaterTime()
    {
        var inst = new Actuality<string>();
        var item = "item";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var noItemLeft = inst.PurgeItemsActualEnough(2000);
        var time2 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.That(noItemLeft);

        Assert.That(time1 == 1000);
        Assert.That(time2 == 0);
    }

    [Test]
    public void TestAddOrUpdateThenRemoveSingleItemLowerTime()
    {
        var inst = new Actuality<string>();
        var item = "item";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var noItemLeft = inst.PurgeItemsActualEnough(500);
        var time2 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.False(noItemLeft);

        Assert.That(time1 == 1000);
        Assert.That(time2 == 1000);
    }

    [Test]
    public void TestAddOrUpdateTwoItemsThenRemoveNewer()
    {
        var inst = new Actuality<string>();
        var item = "item";
        var item2 = "item2";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var noItemLeft = inst.PurgeItemsActualEnough(1000);
        var time2 = inst.Time;
        var addOrUpdate2 = inst.TryAddOrUpdate(item2, 500);
        var time3 = inst.Time;
        var noItemLeft2 = inst.PurgeItemsActualEnough(500);
        var time4 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.That(noItemLeft);
        Assert.That(addOrUpdate2);
        Assert.That(noItemLeft2);

        Assert.That(time1 == 1000);
        Assert.That(time2 == 0);
        Assert.That(time3 == 500);
        Assert.That(time4 == 0);
    }

    [Test]
    public void TestAddOrUpdateItemRemoveThenAddAndRemoveAgainWithLowerTime()
    {
        var inst = new Actuality<string>();
        var item = "item";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var noItemLeft = inst.PurgeItemsActualEnough(1000);
        var time2 = inst.Time;
        var addOrUpdate2 = inst.TryAddOrUpdate(item, 500);
        var time3 = inst.Time;
        var noItemLeft2 = inst.PurgeItemsActualEnough(500);
        var time4 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.That(noItemLeft);
        Assert.That(addOrUpdate2);
        Assert.That(noItemLeft2);

        Assert.That(time1 == 1000);
        Assert.That(time2 == 0);
        Assert.That(time3 == 500);
        Assert.That(time4 == 0);
    }

    [Test]
    public void TestAddOrUpdateTwoItemsThenRemoveFifo()
    {
        var inst = new Actuality<string>();
        var item = "item";
        var item2 = "item2";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var addOrUpdate2 = inst.TryAddOrUpdate(item2, 2000);
        var time2 = inst.Time;
        var noItemLeft = inst.PurgeItemsActualEnough(1000);
        var time3 = inst.Time;
        var noItemLeft2 = inst.PurgeItemsActualEnough(1500);
        var time4 = inst.Time;
        var noItemLeft3 = inst.PurgeItemsActualEnough(2000);
        var time5 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.That(addOrUpdate2);
        Assert.False(noItemLeft);
        Assert.False(noItemLeft2);
        Assert.That(noItemLeft3);

        Assert.That(time1 == 1000);
        Assert.That(time2 == 2000);
        Assert.That(time3 == 2000);
        Assert.That(time4 == 2000);
        Assert.That(time5 == 0);
    }

    [Test]
    public void TestAddOrUpdateTwoItemsThenRemoveFifoTimeFromPast()
    {
        var inst = new Actuality<string>();
        var item = "item";
        var item2 = "item2";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var addOrUpdate2 = inst.TryAddOrUpdate(item2, 500);
        var time2 = inst.Time;
        var noItemLeft = inst.PurgeItemsActualEnough(1000);
        var time3 = inst.Time;
        var noItemLeft2 = inst.PurgeItemsActualEnough(500);
        var time4 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.False(addOrUpdate2); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        Assert.That(noItemLeft);
        Assert.False(noItemLeft2);  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        Assert.That(time1 == 1000);
        Assert.That(time2 == 1000);
        Assert.That(time3 == 0);
        Assert.That(time4 == 0);
    }

    [Test]
    public void TestAddOrUpdateTwoItemsThenRemoveBothLifo()
    {
        var inst = new Actuality<string>();
        var item = "item";
        var item2 = "item2";

        var addOrUpdate = inst.TryAddOrUpdate(item, 1000);
        var time1 = inst.Time;
        var addOrUpdate2 = inst.TryAddOrUpdate(item2, 2000);
        var time2 = inst.Time;
        var noItemLeft = inst.PurgeItemsActualEnough(2000);
        var time3 = inst.Time;
        var noItemLeft2 = inst.PurgeItemsActualEnough(1000);
        var time4 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.That(addOrUpdate2);
        Assert.That(noItemLeft);
        Assert.False(noItemLeft2);

        Assert.That(time1 == 1000);
        Assert.That(time2 == 2000);
        Assert.That(time3 == 0);
        Assert.That(time4 == 0);
    }

    [Test]
    public void TestAddOrUpdateThreeItemsThenRemoveFifo()
    {
        var inst = new Actuality<string>();
        var item = "item";
        var item2 = "item2";
        var item3 = "item3";

        var addOrUpdate = inst.TryAddOrUpdate(item, 3000);
        var time1 = inst.Time;
        var addOrUpdate2 = inst.TryAddOrUpdate(item2, 2000);
        var time2 = inst.Time;
        var noItemLeft1 = inst.PurgeItemsActualEnough(3000);
        var time3 = inst.Time;
        var addOrUpdate3 = inst.TryAddOrUpdate(item3, 1000);
        var time4 = inst.Time;
        var noItemLeft2 = inst.PurgeItemsActualEnough(2000);
        var time5 = inst.Time;
        var noItemLeft3 = inst.PurgeItemsActualEnough(1000);
        var time6 = inst.Time;

        Assert.That(addOrUpdate);
        Assert.False(addOrUpdate2);
        Assert.That(addOrUpdate3);
        Assert.That(noItemLeft1);
        Assert.That(noItemLeft2);
        Assert.False(noItemLeft3);

        Assert.That(time1 == 3000);
        Assert.That(time2 == 3000);
        Assert.That(time3 == 0);
        Assert.That(time4 == 1000);
        Assert.That(time5 == 0);
        Assert.That(time6 == 0);
    }

    [Test]
    public void TestAddOrUpdateThreeItemsThenRemoveFifo2()
    {
        var inst = new Actuality<string>();
        var item = "item";
        var item2 = "item2";
        var item3 = "item3";

        var addedOrUpdated = inst.TryAddOrUpdate(item, 2000);
        var time1 = inst.Time;
        var addedOrUpdated2 = inst.TryAddOrUpdate(item2, 1000);
        var time2 = inst.Time;
        var noItemLeft1 = inst.PurgeItemsActualEnough(1000);
        var time3 = inst.Time;
        var addedOrUpdated3 = inst.TryAddOrUpdate(item3, 3000);
        var time4 = inst.Time;
        var noItemLeft2 = inst.PurgeItemsActualEnough(2000);
        var time5 = inst.Time;
        var noItemLeft3 = inst.PurgeItemsActualEnough(3000);
        var time6 = inst.Time;

        Assert.That(addedOrUpdated);
        Assert.False(addedOrUpdated2);
        Assert.That(addedOrUpdated3);
        Assert.False(noItemLeft1);
        Assert.False(noItemLeft2);
        Assert.That(noItemLeft3);

        Assert.That(time1 == 2000);
        Assert.That(time2 == 2000);
        Assert.That(time3 == 2000);
        Assert.That(time4 == 3000);
        Assert.That(time5 == 3000);
        Assert.That(time6 == 0);
    }

    //[Test, Timeout(30000)]
    public void TestAddOrUpdateItemMultipleThreads()
    {
        var time = 1000;
        var inst = new Actuality<string>();
        var item = "item";

        var threadCount = (Environment.ProcessorCount - 1) / 2 + 1;
        Thread[] threads = new Thread[threadCount];
        using var signal = new ManualResetEventSlim(false, 2000);
        using var signal2 = new ManualResetEventSlim(false, 2000);
        var running = true;
        var n = 0;
        var m = 0;
        var q = 0;

        for (int i = 0; i < threadCount; i++)
        {
            threads[i] = new Thread(j =>
            {
                var k = (int)j;
                var t = k + time + Interlocked.CompareExchange(ref q, 0, 0);
                var itemk = item + k;
                while (running)
                {
                    Interlocked.Increment(ref n);
                    signal.Wait();

                    inst.TryAddOrUpdate(itemk, t);

                    Interlocked.Increment(ref m);
                    signal2.Wait();
                    t = k + time + Interlocked.CompareExchange(ref q, 0, 0);
                }
            });

            threads[i].Start(i);
        }

        var failed = false;
        var resultTime = 0L;
        for (; q < 10_000_000;)
        {
            while (n < threadCount)
                Thread.SpinWait(40);

            n = 0;
            signal2.Reset();
            signal.Set();

            while (m < threadCount)
                Thread.SpinWait(40);

            m = 0;
            signal.Reset();
            Interlocked.Add(ref q, threadCount);
            signal2.Set();

            if (inst.Time == time + q - 1) continue;
            failed = true;
            break;
        }

        running = false;
        signal.Set();
        signal2.Set();
        for (int i = 0; i < threadCount; i++)
            threads[i].Join();

        if (failed)
            Assert.AreEqual(time + q - 1, resultTime);
    }

    [Test, Timeout(30000)]
    public void TestPerformance()
    { 
        // Intel(R) Xeon(R) CPU E5-2680 v2 @ 2.80GHz, single core running at 3600MHz
        // Test took: 00:00:04.1394929

        var inst = new Actuality<string>();
        var item1 = "item1";
        var item2 = "item2";
        var item3 = "item3";
        var item4 = "item4";
        var item5 = "item5";
        var sw = new Stopwatch();

        var n = 10_000_000;
        var k = 1;
        var items = new long[n * 5];

        for (long i = 0; i < n * 5; i++)
        {
            items[i] = i;
        }

        for (long i = 0; i < n; i += k * 5)
        {
            for (int j = 0; j < k * 5; j += 5)
            {
                var m1 = items[i + j];
                var m2 = items[i + j + 1];
                var m3 = items[i + j + 2];
                var m4 = items[i + j + 3];
                var m5 = items[i + j + 4];
                sw.Start();
                inst.TryAddOrUpdate(item1, m1);
                inst.TryAddOrUpdate(item3, m3);
                inst.TryAddOrUpdate(item2, m2);
                inst.TryAddOrUpdate(item5, m5);
                inst.TryAddOrUpdate(item4, m4);
                sw.Stop();
            }

            for (int j = 0; j < k * 5; j += 5)
            {
                var m1 = items[i + j + 3];
                sw.Start();
                inst.PurgeItemsActualEnough(m1);
                sw.Stop();
            }
        } 

        Assert.Pass($"Test took: {sw.Elapsed}");
    }
}