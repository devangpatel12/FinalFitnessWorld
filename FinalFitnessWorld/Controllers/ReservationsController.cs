﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalFitnessWorld.Models;
using FinalFitnessWorld.Utils;
using Microsoft.AspNet.Identity;

namespace FinalFitnessWorld.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private ReservationModels db = new ReservationModels();

        // GET: Reservations
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var reservations = db.Reservations.Include(r => r.Branch1).Include(r => r.Customer1).Include(r => r.Trainer1);
            if (User.IsInRole("trainer"))
            {
                reservations = db.Reservations.Where(s => s.Trainer == userId);
            }
            else if (User.IsInRole("customer"))
            {
                reservations = db.Reservations.Where(s => s.Customer == userId);
            }
            return View(reservations.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }
        /*
                // GET: Reservations/Create
                public ActionResult Create()
                {
                    ViewBag.Branch = new SelectList(db.Branches, "Id", "Name");
                    ViewBag.Customer = new SelectList(db.Customers, "Id", "Name");
                    ViewBag.Trainer = new SelectList(db.Trainers, "Id", "Name");
                    return View();
                }

                // POST: Reservations/Create
                // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
                // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult Create([Bind(Include = "Id,Customer,Branch,Trainer,Date,Time,ReservationStatus")] Reservation reservation)
                {
                    if (ModelState.IsValid)
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    ViewBag.Branch = new SelectList(db.Branches, "Id", "Name", reservation.Branch);
                    ViewBag.Customer = new SelectList(db.Customers, "Id", "Name", reservation.Customer);
                    ViewBag.Trainer = new SelectList(db.Trainers, "Id", "Name", reservation.Trainer);
                    return View(reservation);
                }
        */

        // GET: Reservations/Create
        public ActionResult Create()
        {
            ViewBag.Branch = new SelectList(db.Branches, "Id", "Name");
            //ViewBag.Customer = new SelectList(db.Customers, "Id", "Name");
            ViewBag.Trainer = new SelectList(" ");
            return View();
        }

        //custom code
        public JsonResult GetTrainerList(int Branch)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Trainer> TrainerList = db.Trainers.Where(x => x.Branch == Branch).ToList();
            return Json(TrainerList, JsonRequestBehavior.AllowGet);
        }
        //custom code

        //custom code
        public JsonResult GetReservationList(DateTime date,String trainer)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Reservation> ReservationList = db.Reservations.Where(x => x.Date == date && x.Trainer == trainer && (x.ReservationStatus != "Disapproved")).ToList();
            return Json(ReservationList, JsonRequestBehavior.AllowGet);
        }
        //custom code


        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Branch,Trainer,Date,Time")] Reservation reservation)
        {
            reservation.Customer = User.Identity.GetUserId();
            reservation.ReservationStatus = "Pending";
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Branch = new SelectList(db.Branches, "Id", "Name", reservation.Branch);
            //ViewBag.Customer = new SelectList(db.Customers, "Id", "Name", reservation.Customer);
            ViewBag.Trainer = new SelectList(db.Trainers.Where(x => x.Branch == reservation.Branch), "Id", "Name", reservation.Trainer);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Branch = new SelectList(db.Branches, "Id", "Name", reservation.Branch);
            //ViewBag.Customer = new SelectList(db.Customers, "Id", "Name", reservation.Customer);
            ViewBag.Trainer = new SelectList(db.Trainers.Where(x => x.Branch == reservation.Branch), "Id", "Name", reservation.Trainer);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Customer,Branch,Trainer,Date,Time,ReservationStatus")] Reservation reservation)
        {
            if (User.IsInRole("customer"))
            {
                reservation.ReservationStatus = "Pending";
            }
            if (ModelState.IsValid)
            {
                if (reservation.ReservationStatus.Equals("Approved"))
                {
                    Customer customer = db.Customers.Find(reservation.Customer);
                    String toEmail = customer.Email;
                    String subject = "Approval Notification";
                    String contents = "Your Reservation Booking on " + reservation.Date.Date + ", " + reservation.Time + " with our trainer has been Approved";

                    EmailSender es = new EmailSender();
                    es.Send(toEmail, subject, contents);
                }

                if (reservation.ReservationStatus.Equals("Disapproved"))
                {
                    Customer customer = db.Customers.Find(reservation.Customer);
                    String toEmail = customer.Email;
                    String subject = "Disapproval Notification";
                    String contents = "Your Reservation Booking on " + reservation.Date.Date + ", " + reservation.Time + " with our trainer has been Disapproved";

                    EmailSender es = new EmailSender();
                    es.Send(toEmail, subject, contents);
                }
                    
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Branch = new SelectList(db.Branches, "Id", "Name", reservation.Branch);
            //ViewBag.Customer = new SelectList(db.Customers, "Id", "Name", reservation.Customer);
            ViewBag.Trainer = new SelectList(db.Trainers.Where(x => x.Branch == reservation.Branch), "Id", "Name", reservation.Trainer);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
