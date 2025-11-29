


    <!-- jquery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <script src="~/Scripts/jquery-3.5.1.min.js"></script>


view for jquey
  @*/******************** JQUERY EDIT ****************/*@
  @*/*************************************************/*@
  <div class="mb-3">
      <label class="fw-bold">Article Title</label>
      <div class="input-group">
          <input type="text" id="title_@Model.id" value="@Model.articletitle" class="form-control">
          <button class="btn btn-sm btn-outline-success" type="button" onclick="saveTitle('@Model.id')">Save</button>
      </div>
      <span id="msg_@Model.id" class="text-danger"></span>
  </div>

  <script>
  function saveTitle(id) {
      var txt = $("#title_" + id).val();
      $.post("@Url.Action("UpdateTitle","Adminfaqs")", { id: id, newTitle: txt }, function (res) {
          var ok = res === "Success";
          $("#title_" + id).css("border-color", ok ? "green" : "red");
          $("#msg_" + id).text(ok ? "" : res);
      });
  }
  </script>
  @*/*****************************************************************/*@
  @*/*****************************************************************/*@


  Controller
     /******************** JQUERY EDIT ****************/
   /*************************************************/
   [HttpPost]
   public JsonResult UpdateTitle(string id, string newTitle)
   {
       try
       {
           if (string.IsNullOrWhiteSpace(newTitle))
               return Json("Title cannot be empty.", JsonRequestBehavior.AllowGet);

           var faq = db.Adminfaqs.Find(id);
           if (faq == null) return Json("Record not found.", JsonRequestBehavior.AllowGet);

           faq.articletitle = newTitle.Trim();
           faq.updatedate = DateTime.Now;
           db.SaveChanges();

           return Json("Success", JsonRequestBehavior.AllowGet);
       }
       catch (Exception ex)
       {
           return Json("Save failed: " + ex.Message, JsonRequestBehavior.AllowGet);
       }
   }
   /*****************************************************************/