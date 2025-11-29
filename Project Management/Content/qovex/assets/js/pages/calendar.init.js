/*!function(v){"use strict";function e(){}e.prototype.init=function(){var a=v("#event-modal"),t=v("#modal-title"),n=v("#form-event"),l=null,i=null,r=document.getElementsByClassName("needs-validation"),l=null,i=null,e=new Date,s=e.getDate(),d=e.getMonth(),e=e.getFullYear();new FullCalendarInteraction.Draggable(document.getElementById("external-events"),{itemSelector:".external-event",eventData:function(e){return{title:e.innerText,className:v(e).data("class")}}});e=[{title:"All Day Event",start:new Date(e,d,1)},{title:"Long Event",start:new Date(e,d,s-5),end:new Date(e,d,s-2),className:"bg-warning"},{id:999,title:"Repeating Event",start:new Date(e,d,s-3,16,0),allDay:!1,className:"bg-info"},{id:999,title:"Repeating Event",start:new Date(e,d,s+4,16,0),allDay:!1,className:"bg-primary"},{title:"Meeting",start:new Date(e,d,s,10,30),allDay:!1,className:"bg-success"},{title:"Lunch",start:new Date(e,d,s,12,0),end:new Date(e,d,s,14,0),allDay:!1,className:"bg-danger"},{title:"Birthday Party",start:new Date(e,d,s+1,19,0),end:new Date(e,d,s+1,22,30),allDay:!1,className:"bg-success"},{title:"Click for Google",start:new Date(e,d,28),end:new Date(e,d,29),url:"http://google.com/",className:"bg-dark"}],document.getElementById("external-events"),d=document.getElementById("calendar");function o(e){a.modal("show"),n.removeClass("was-validated"),n[0].reset(),v("#event-title").val(),v("#event-category").val(),t.text("Add Event"),i=e}var c=new FullCalendar.Calendar(d,{plugins:["bootstrap","interaction","dayGrid","timeGrid"],editable:!0,droppable:!0,selectable:!0,defaultView:"dayGridMonth",themeSystem:"bootstrap",header:{left:"prev,next today",center:"title",right:"dayGridMonth,timeGridWeek,timeGridDay,listMonth"},eventClick:function(e){a.modal("show"),n[0].reset(),l=e.event,v("#event-title").val(l.title),v("#event-category").val(l.classNames[0]),i=null,t.text("Edit Event"),i=null},dateClick:function(e){o(e)},events:e});c.render(),v(n).on("submit",function(e){e.preventDefault();v("#form-event :input");var t=v("#event-title").val(),e=v("#event-category").val();!1===r[0].checkValidity()?(event.preventDefault(),event.stopPropagation(),r[0].classList.add("was-validated")):(l?(l.setProp("title",t),l.setProp("classNames",[e])):(e={title:t,start:i.date,allDay:i.allDay,className:e},c.addEvent(e)),a.modal("hide"))}),v("#btn-delete-event").on("click",function(e){l&&(l.remove(),l=null,a.modal("hide"))}),v("#btn-new-event").on("click",function(e){o({date:new Date,allDay:!0})})},v.CalendarPage=new e,v.CalendarPage.Constructor=e}(window.jQuery),function(){"use strict";window.jQuery.CalendarPage.init()}();*/




//!function (v) { "use strict"; function e() { } e.prototype.init = function () { var a = v("#event-modal"), t = v("#modal-title"), n = v("#form-event"), l = null, i = null, r = document.getElementsByClassName("needs-validation"), l = null, i = null, e = new Date, s = e.getDate(), d = e.getMonth(), e = e.getFullYear(); new FullCalendarInteraction.Draggable(document.getElementById("external-events"), { itemSelector: ".external-event", eventData: function (e) { return { title: e.innerText, className: v(e).data("class") } } }); document.getElementById("external-events"), d = document.getElementById("calendar"); function o(e) { a.modal("show"), n.removeClass("was-validated"), n[0].reset(), v("#event-title").val(), v("#event-category").val(), t.text("Add Event"), i = e } var c = new FullCalendar.Calendar(d, { plugins: ["bootstrap", "interaction", "dayGrid", "timeGrid"], editable: !0, droppable: !0, selectable: !0, defaultView: "dayGridMonth", themeSystem: "bootstrap", header: { left: "prev,next today", center: "title", right: "dayGridMonth,timeGridWeek,timeGridDay,listMonth" }, eventClick: function (e) { a.modal("show"), n[0].reset(), l = e.event, v("#event-title").val(l.title), v("#event-category").val(l.classNames[0]), i = null, t.text("Edit Event"), i = null }, dateClick: function (e) { o(e) }, events: [] }); c.render(), v(n).on("submit", function (e) { e.preventDefault(); v("#form-event :input"); var t = v("#event-title").val(), e = v("#event-category").val(); !1 === r[0].checkValidity() ? (event.preventDefault(), event.stopPropagation(), r[0].classList.add("was-validated")) : (l ? (l.setProp("title", t), l.setProp("classNames", [e])) : (e = { title: t, start: i.date, allDay: i.allDay, className: e }, c.addEvent(e)), a.modal("hide")) }), v("#btn-delete-event").on("click", function (e) { l && (l.remove(), l = null, a.modal("hide")) }), v("#btn-new-event").on("click", function (e) { o({ date: new Date, allDay: !0 }) }) }, v.CalendarPage = new e, v.CalendarPage.Constructor = e }(window.jQuery), function () { "use strict"; window.jQuery.CalendarPage.init() }();


!function ($) {
    "use strict";

    function CalendarPage() { }

    CalendarPage.prototype.init = function () {
        var $modal = $("#event-modal"),
            $modalTitle = $("#modal-title"),
            $form = $("#form-event"),
            currentEvent = null,
            selectedDate = null;

        // Make external events draggable
        new FullCalendarInteraction.Draggable(document.getElementById("external-events"), {
            itemSelector: ".external-event",
            eventData: function (el) {
                return {
                    title: el.innerText,
                    className: $(el).data("class")
                };
            }
        });

        // Initialize FullCalendar
        var calendarEl = document.getElementById("calendar");
        var calendar = new FullCalendar.Calendar(calendarEl, {
            plugins: ["bootstrap", "interaction", "dayGrid", "timeGrid", "list"],
            themeSystem: "bootstrap",
            editable: true,
            droppable: true,
            selectable: true,
            headerToolbar: {
                left: "prev,next today",
                center: "title",
                right: "dayGridMonth,timeGridWeek,timeGridDay,listMonth"
            },
            dateClick: function (info) {
                currentEvent = null;
                selectedDate = info;
                $modalTitle.text("Add Event");
                $form[0].reset();
                $form.removeClass("was-validated");
                $("#event-title").val('');
                $("#event-category").val('bg-danger');
                $('#btn-delete-event').hide();
                $modal.modal("show");
            },
            eventClick: function (info) {
                currentEvent = info.event;
                $modalTitle.text("Edit Event");
                $form[0].reset();
                $form.removeClass("was-validated");

                // Set event title and category
                $("#event-title").val(currentEvent.title);
                $("#event-category").val(currentEvent.classNames[0]);

                // Convert start and end dates to 'YYYY-MM-DDTHH:mm' format
                function formatDateTimeLocal(date) {
                    if (!date) return '';
                    const pad = n => n.toString().padStart(2, '0');
                    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
                }

                $("#event-start").val(formatDateTimeLocal(currentEvent.start));
                $("#event-end").val(formatDateTimeLocal(currentEvent.end || currentEvent.start));

                $('#btn-delete-event').show();
                $modal.modal("show");
            },
            eventDrop: function (info) {
                var eventData = {
                    EventID: info.event.id,
                    StartDateTime: info.event.start.toISOString(),
                    EndDateTime: info.event.end ? info.event.end.toISOString() : info.event.start.toISOString(),
                    ThemeColor: info.event.classNames[0]
                };

                $.ajax({
                    url: '/Calendars/EditEvent',
                    type: 'POST',
                    data: JSON.stringify(eventData),
                    contentType: 'application/json',
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                icon: 'success',
                                title: response.message,
                                toast: true,
                                position: 'top-end',
                                timer: 2500,
                                showConfirmButton: false
                            });
                        } else {
                            Swal.fire({ icon: 'error', title: 'Error', text: response.message });
                        }
                    }
                });
            },
            events: function (fetchInfo, successCallback, failureCallback) {
                $.ajax({
                    url: '/Calendars/GetEvents',
                    type: 'GET',
                    success: function (events) {
                        console.log("Events loaded from server:", events);
                        successCallback(events);
                    },
                    error: function (xhr, status, error) {
                        console.error("Error loading events:", status, error);
                        failureCallback();
                    }
                });
            }
        });
        calendar.render();

        // Load events dynamically into calendar AND sidebar
        function loadExternalEvents() {
            $.ajax({
                url: '/Calendars/GetEvents',
                type: 'GET',
                success: function (events) {
                    // Clear sidebar events
                    var container = $('#external-events');
                    container.find('.external-event').remove();

                    // Add sidebar events
                    events.forEach(function (ev) {
                        var eventHtml = `<div class="external-event fc-event ${ev.className}" data-id="${ev.id}" data-title="${ev.title}" data-class="${ev.className}">
                            <i class="mdi mdi-checkbox-blank-circle font-size-11 me-2"></i>${ev.title}
                        </div>`;
                        container.append(eventHtml);
                    });

                    // Clear calendar events
                    calendar.getEvents().forEach(e => e.remove());

                    // Add events to calendar
                    events.forEach(function (ev) {
                        calendar.addEvent({
                            id: ev.id,
                            title: ev.title,
                            start: ev.start,
                            end: ev.end,
                            allDay: ev.allDay || false,
                            className: ev.className
                        });
                    });
                },
                error: function () {
                    console.error('Failed to load external events.');
                }
            });
        }

        loadExternalEvents();

        // Form submit (Add / Edit)
        $form.on("submit", function (e) {
            e.preventDefault();
            var title = $("#event-title").val().trim();
            var category = $("#event-category").val();
            var start = $("#event-start").val();
            var end = $("#event-end").val();

            // Manually parse datetime-local input values
            var startDate = new Date(start.replace('T', ' '));
            var endDate = new Date(end.replace('T', ' '));

            console.log("Event Title:", title);
            console.log("Event Category:", category);
            console.log("Start Date & Time from input:", start);
            console.log("End Date & Time from input:", end);
            console.log("Parsed Start Date & Time:", startDate);
            console.log("Parsed End Date & Time:", endDate);

            if (!title || isNaN(startDate.getTime()) || isNaN(endDate.getTime())) {
                Swal.fire({ icon: 'warning', title: 'Missing Fields', text: 'Please provide a valid event name and dates.' });
                return;
            }

            var eventData = {
                EventID: currentEvent ? currentEvent.id : null,
                Subject: title,
                StartDateTime: startDate.toISOString(),
                EndDateTime: endDate.toISOString(),
                ThemeColor: category
            };

            console.log("Event Data to be sent:", eventData);

            const url = currentEvent ? '/Calendars/EditEvent' : '/Calendars/CreateEvent';

            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify(eventData),
                contentType: 'application/json',
                success: function (response) {
                    console.log("Server Response:", response);
                    if (response.success) {
                        $modal.modal('hide');
                        Swal.fire({
                            icon: 'success',
                            title: response.message,
                            toast: true,
                            position: 'top-end',
                            timer: 2500,
                            showConfirmButton: false
                        });
                        loadExternalEvents();
                    } else {
                        Swal.fire({ icon: 'error', title: 'Error', text: response.message });
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error:", status, error);
                    Swal.fire({ icon: 'error', title: 'AJAX Error', text: error });
                }
            });
        });

        // Delete event
        $('#btn-delete-event').on('click', function () {
            if (!currentEvent) return;
            Swal.fire({
                title: 'Are you sure?',
                text: "This event will be permanently deleted.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: '/Calendars/DeleteEvent',
                        type: 'DELETE',
                        data: JSON.stringify({ eventId: currentEvent.id }),
                        contentType: 'application/json',
                        success: function (response) {
                            if (response.success) {
                                $modal.modal('hide');
                                Swal.fire({
                                    icon: 'success',
                                    title: 'Deleted!',
                                    text: response.message,
                                    toast: true,
                                    position: 'top-end',
                                    timer: 2000,
                                    showConfirmButton: false
                                });
                                loadExternalEvents();
                            } else {
                                Swal.fire({ icon: 'error', title: 'Error', text: response.message });
                            }
                        }
                    });
                }
            });
        });
    };

    window.jQuery.CalendarPage = new CalendarPage();
    window.jQuery.CalendarPage.Constructor = CalendarPage;

}(window.jQuery);

(function () {
    "use strict";
    window.jQuery.CalendarPage.init();
})();