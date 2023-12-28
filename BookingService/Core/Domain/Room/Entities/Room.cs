﻿using Domain.Room.Exceptions;
using Domain.Room.Ports;
using Domain.Room.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Room.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public Price Price { get; set; }

        public bool IsAvaliable
        {
            get
            {
                if (!InMaintenance || HasGuest)
                {
                    return false;
                }
                return true;
            }
        }

        public bool HasGuest
        {
            get { return true; }
        }

        private void ValidateState()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidRoomDataException();
            }
        }

        public async Task Save(IRoomRepository roomRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                this.Id = await roomRepository.Create(this);
            }
            else
            {
                this.Id = await roomRepository.Update(this);
            }
        }
    }
}