using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkPathAPI.Controllers;
using ParkPathAPI.Mapper;
using ParkPathAPI.Models;
using ParkPathAPI.Repository.IRepository;
using Xunit;

namespace ParkPathAPI.UnitTests
{
    public class NationalParkControllerTests
    {
        private Mock<INationalParkRepository> _npRepository;
        private NationalParksController _controller;
        private NationalParkDto _nationalParkDto;

        public NationalParkControllerTests()
        {
            _npRepository = new Mock<INationalParkRepository>();

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new ParkPathMappings()); });
            var mapper = mockMapper.CreateMapper();
            
            _controller = new NationalParksController(_npRepository.Object, mapper);

            _nationalParkDto = new NationalParkDto()
            {
                Id = 5,
                Name = "DTO Park",
                State = "NY"
            };
        }

        [Fact]
        public void GetNationalParks_WhenCalled_ReturnsListOfNationalParks()
        {
            _npRepository.Setup(repo => repo.GetNationalParks()).Returns(GetFakeNationalParks());

            var result = _controller.GetNationalParks();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<NationalParkDto>>(okResult.Value); 
            var firstPark = returnValue.FirstOrDefault(); 
            Assert.Equal("TestPark1", firstPark.Name);
        }
        
        [Fact]
        public void GetNationalPark_WhenCalled_ReturnsRightPark()
        {
            var nationalPark = GetFakeNationalParks().SingleOrDefault(x => x.Id == 2);
            _npRepository.Setup(repo => repo.GetNationalPark(2)).Returns(nationalPark);

            var result = _controller.GetNationalPark(2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<NationalParkDto>(okResult.Value);
            Assert.Equal(nationalPark.Name, returnValue.Name);
        }
        
        [Fact]
        public void GetNationalPark_WhenParkIsNull_ReturnsNotFound()
        {
            _npRepository.Setup(repo => repo.GetNationalPark(It.IsAny<int>())).Returns((NationalPark) null);

            var result = _controller.GetNationalPark(It.IsAny<int>());

            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public void DeleteNationalPark_NoParkInDatabase_ReturnsNotFound()
        {
            _npRepository.Setup(repo => repo.NationalParkExists(It.IsAny<int>())).Returns(false);
            
            var result = _controller.DeleteNationalPark(It.IsAny<int>());
            
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public void DeleteNationalPark_SuccessfulDetele_ReturnsNoContent()
        {
            var nationalPark = GetFakeNationalParks().SingleOrDefault(x => x.Id == 1);
            _npRepository.Setup(repo => repo.NationalParkExists(1)).Returns(true);
            _npRepository.Setup(repo => repo.GetNationalPark(1)).Returns(nationalPark);
            _npRepository.Setup(repo => repo.DeleteNationalPark(nationalPark)).Returns(true);
            
            var result = _controller.DeleteNationalPark(1);
            
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public void CreateNationalPark_SuccessfulAddition_ReturnsCreatedAtRouteWithAddedModel()
        {
            _npRepository.Setup(repo => repo.NationalParkExists(It.IsAny<int>())).Returns(false);
            _npRepository.Setup(repo => repo.CreateNationalPark(It.IsAny<NationalPark>())).Returns(true);

            var result = _controller.CreateNationalPark(_nationalParkDto);
            
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<NationalPark>(createdAtRouteResult.Value);
            Assert.Equal(_nationalParkDto.Id, returnValue.Id);
        }
        
        private ICollection<NationalPark> GetFakeNationalParks()
        {
            return new List<NationalPark>()
            {
                new NationalPark()
                {
                    Id = 1,
                    Name = "TestPark1",
                    State = "State1"
                },
                new NationalPark()
                {
                    Id = 2,
                    Name = "TestPark2",
                    State = "State2"
                }
            };
        }
    }
}