import { Directive, HostListener, OnDestroy } from '@angular/core';

declare var $: any;

@Directive({
  selector: '[appUisectionbox]'
})
export class UisectionboxDirective implements OnDestroy {

  constructor() {
    $(document).off('click', '.box header .actions i.box_toggle');
    $(document).off('click', '.box header .actions i.box_close');
    $(document).on('click', '.box header .actions i.box_toggle', function (e) {
      var _this = $(this);
      var txt = _this.html();
      if (txt == "expand_more") {
        _this.html("expand_less") && e.preventDefault();
      } else if (txt == "expand_less") {
        _this.html("expand_more") && e.preventDefault();
      }
      _this.parent().parent().parent().toggleClass("collapsed");
    });

    $(document).on('click', '.box header .actions i.box_close', function (e) {
      var _this = $(this);
      _this.parent().parent().parent().addClass("hide").hide() && e.preventDefault();
    });
  }

  ngOnDestroy(): void {
    $(document).off('click', '.box header .actions i.box_toggle', function(e){});
    $(document).off('click', '.box header .actions i.box_close', function(e){});
  }
}
