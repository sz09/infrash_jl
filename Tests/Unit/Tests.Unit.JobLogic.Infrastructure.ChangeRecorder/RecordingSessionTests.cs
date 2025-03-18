using JobLogic.Infrastructure.ChangeRecorder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JobLogic.Infrastructure.Contract.Extensions;
using FluentAssertions;

namespace Tests.Unit.JobLogic.Infrastructure.ChangeRecorder
{
    [TestClass]
    public class RecordingSessionTests
    {

        public class Model
        {
            public ChangeRecord<int> Id { get; set; }
            public ChangeRecord<string> Name { get; set; }

            public InnerModel InnerModel { get; set; }

            public double Money { get; set; }
        }

        public class InnerModel
        {
            public ChangeRecord<int> Id { get; set; }
            public ChangeRecord<string> Na { get; set; }
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeTrue_WhenRecordDifferentValue()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew");
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.Name);
            nameRC.IsModified().Should().BeTrue();
            nameRC.OriginValue.Should().Be("nameOld");
            nameRC.CurrentValue.Should().Be("nameNew");
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeFalse_WhenRecordSameValue()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameOld");
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.Name);
            nameRC.IsModified().Should().BeFalse();
            nameRC.OriginValue.Should().Be("nameOld");
            nameRC.CurrentValue.Should().Be("nameOld");
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeFalse_WhenRecordChangeBackAndForthToSameValue()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew");
            session.Record(x => x.Name, "nameOld");
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.Name);
            nameRC.IsModified().Should().BeFalse();
            nameRC.OriginValue.Should().Be("nameOld");
            nameRC.CurrentValue.Should().Be("nameOld");
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeTrue_WhenRecordChangeBackAndForthToDifferntValue()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew");
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew2");
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.Name);
            nameRC.IsModified().Should().BeTrue();
            nameRC.OriginValue.Should().Be("nameOld");
            nameRC.CurrentValue.Should().Be("nameNew2");
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeTrue_WhenInvokeFinalizeAndStartNewSessionThenRecordChange()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew");
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew2");
            session.FinalizeAndStartNewSession();
            session.Record(x => x.Name, "nameNew3");
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.Name);
            nameRC.IsModified().Should().BeTrue();
            nameRC.OriginValue.Should().Be("nameNew2");
            nameRC.CurrentValue.Should().Be("nameNew3");
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeFalse_WhenInvokeFinalizeAndStartNewSessionThenRecordChangeWithSameValue()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew");
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew2");
            session.FinalizeAndStartNewSession();
            session.Record(x => x.Name, "nameNew2");
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.Name);
            nameRC.IsModified().Should().BeFalse();
            nameRC.OriginValue.Should().Be("nameNew2");
            nameRC.CurrentValue.Should().Be("nameNew2");
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeFalse_WhenInvokeFinalizeAndStartNewSessionWithoutRecordChange()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew");
            session.Record(x => x.Name, "nameOld");
            session.Record(x => x.Name, "nameNew2");
            session.FinalizeAndStartNewSession();
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.Name);
            nameRC.IsModified().Should().BeFalse();
            nameRC.OriginValue.Should().Be("nameNew2");
            nameRC.CurrentValue.Should().Be("nameNew2");
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeFalse_WhenAccessNestedProperty()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.InnerModel.Na, "nameOld");
            session.Record(x => x.InnerModel.Na, "nameNew");
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.InnerModel.  /* intentionally comment to test case when access prop syntax is differ */ Na);
            nameRC.IsModified().Should().BeTrue();
            nameRC.OriginValue.Should().Be("nameOld");
            nameRC.CurrentValue.Should().Be("nameNew");
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeFalse_WhenRecordNothing()
        {
            var session = new RecordingSession<Model>();
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.InnerModel.  /* intentionally comment to test case when access prop syntax is differ */ Na);
            nameRC.IsModified().Should().BeFalse();
            nameRC.Should().BeNull();
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldBeFalse_WhenRecordFirstTime()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.Name, "nameOld");
            var rs = session.FinalizeAndStartNewSession();
            var nameRC = rs.PickChangeRecord(y => y.Name);
            nameRC.IsModified().Should().BeFalse();
            nameRC.OriginValue.Should().Be("nameOld");
            nameRC.CurrentValue.Should().Be("nameOld");
        }

        [TestMethod]
        public void TestPickChangeRecord_IsModifiedShouldWorkFine()
        {
            var session = new RecordingSession<Model>();
            session.Record(x => x.InnerModel.  /* intentionally comment to test case when access prop syntax is differ */ Na, "V1");
            var rs = session.FinalizeAndStartNewSession();
            var NaRc = rs.PickChangeRecord(y => y.InnerModel.Na);
            NaRc.IsModified().Should().BeFalse();
            NaRc.OriginValue.Should().Be("V1");
            NaRc.CurrentValue.Should().Be("V1");

            var nameRC = rs.PickChangeRecord(y => y.Name);
            nameRC.IsModified().Should().BeFalse();
            nameRC.Should().BeNull();
        }
    }
}