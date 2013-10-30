using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beauty;
using Beauty.InterFace;
using Beauty.Model;
using Beauty.Service;

namespace Beauty.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        //[TestMethod]
        //public void Index()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;

        //    // Assert
        //    Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        //}

        //[TestMethod]
        //public void About()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.About() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void Contact()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Contact() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        [TestMethod]
        public void TestCanCreateShare()
        {
            IShare shareservice = new ShareService();
            Share newshare = new Share
            {
                ID = Guid.NewGuid(),

                Bady = new Bady{ ID = Guid.NewGuid()},
                Comment = "Comment",
                Createby = "Admin",
                Createtime = DateTime.Now.ToString(),
                IsSuper = false,
                Keyword = "Keyword",
                Liked = 0,
                Link = "http://www.baidu.com",

                Statues = 1,
                Updateby = "Admin",
                Updatetime = DateTime.Now.ToString(),
                UserId = new Guid("32A2FF15-6AE0-4B7A-8982-C85915738549"),
                Username = "fengzijun"
            };
            Guid id = shareservice.Create(newshare);
            Assert.AreEqual(id, newshare.ID);
        }

         [TestMethod]
        public void TestCanCreateLike()
        {
            ILike likeservice = new LIkeService();
            Like like = new Like
            {
                ID = Guid.NewGuid(),
                Bady = new Bady { ID = Guid.NewGuid() },
                Likednum = 100,
                Likenum = 500,
                Statues = 1,
                Recordnum = 200,
                Lnk = "http://www.baidu.com",
                Supernum = 30,
                Commentnum = 40,
                Username = "fengzijun"


            };
            Guid id = likeservice.Create(like);
            Assert.AreEqual(id, like.ID);
        }

        [TestMethod]
        public void TestCanCreatebady()
         {
             IBady badyservice = new BadyService();
             Bady bady = new Bady
             {
                 ID = Guid.NewGuid(),
                 Price = "330",
                 ImageUrl = "http://img03.taobaocdn.com/bao/uploaded/i3/15334026849221246/T19gWmFgRcXXXXXXXX_!!0-item_pic.jpg",
                 BadyId = "60225012",
                 Badydescription = "kunbu 女包韩版 包包2013新款 女用单肩包 女手提包 女斜挎包大包",
                 Link = "http://detail.tmall.com/item.htm?id=21431643167",
                 Statues = 1,
                 Platfrom = "tmall.com",
                  Username = "fengzijun"
             };
             Guid id = badyservice.Create(bady);
             Assert.AreEqual(id, bady.ID);

         }

        [TestMethod]
        public void TestCanCeateUserStore()
        {
            IUserStore userstoreservice = new UserStoreService();
            UserStore userstore = new UserStore
            {
                ID = Guid.NewGuid(),
                Bady = new Bady { ID = new Guid("3B5B342C-8E09-4854-8C1D-05600B5C9955") } ,
                Comment = 100,
                Liked = 0,
                Rank = 40,
                Statues = 1,
                Record = 40,
                Page = 1,
                Link = "www.baidu.com",
                Username = "fengzijun"

            };
            Guid id = userstoreservice.Create(userstore);
            Assert.AreEqual(id, userstore.ID);
        }

        [TestMethod]
        public void TestCanCreateTask()
        {
            ITask taskservice = new TaskService();
            Task task = new Task
            {
                ID = Guid.NewGuid(),
                IsAuto = false,
                Statues = 1,
                Taskid = new Guid("7F1343B9-851B-4E06-8605-8662B9459AEA"),
                Comment = "不错很好",
                  
                Runstatues = 0,
                Username = "fengzijun",
                TaskType = "record"
            };

            Guid id = taskservice.Create(task);
            Assert.AreEqual(id, task.ID);
        }

        [TestMethod]
        public void TestCanCreateGroup()
        {
            for(var i=0;i<10;i++)
            {
                IGroup groupservice = new GroupService();
                Group group = new Group
                {
                    ID = "50000" + i.ToString(),
                    Username = "fengzijun",
                    Name = "秋装" + i.ToString(),
                    Statues = 1
                };
                string id = groupservice.Create(group);
                Assert.AreEqual(id, group.ID);
            }
        }

        [TestMethod]
        public void TestCanCreateFirstPageArg()
        {
            IFirstPageArg firstpageservice = new FirstPageArgService();
            FirstPageArg firstpage = new FirstPageArg
            {
                ID = Guid.NewGuid(),
                LikeArg = 300,
                RecordArg = 100,
                CommentArg = 100,
                Statues = 1,
                Type = "滑雪衫"
            };

            Guid id = firstpageservice.Create(firstpage);
            Assert.AreEqual(id, firstpage.ID);
        }

        [TestMethod]
        public void GetSetting()
        {
            UserSettingService usersettingservice = new UserSettingService();
            usersettingservice.GetByUsername("fengzijun");
        }
    }
}
