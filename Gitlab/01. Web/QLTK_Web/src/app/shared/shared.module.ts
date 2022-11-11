import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { NgbDateParserFormatter, NgbModule, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {
    DevExtremeModule, DxTreeListModule, DxTreeViewModule, DxDropDownBoxModule, DxContextMenuModule,
    DxDateBoxModule,
    DxDataGridModule
} from 'devextreme-angular';
import { CurrencyMaskModule } from "ng2-currency-mask";

import { ToastrModule } from 'ngx-toastr';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';

import {
    TruncatePipe, FilterInListPipe, FilterBadgeClassInListPipe, FilterTextClassInListPipe, FilterBadgeClassInListChildPipe,
    NtsNumberIntDirective, NtsLocationDirective, DateUtils, AppSetting, UisectionboxDirective
} from '../shared';
import { UipermissionDirective, DiablePermissionDirective, ReadonlyPermissionDirective } from '../shared';
import { NtscurrencyDirective } from './directives/ntscurrency.directive';
import { NtscurrencyPipe } from './pipe/ntscurrency.pipe';
import { NtsNumberDirective } from './directives/ntsnumber.directive';
import { PagingComponent } from './paging/paging.component';
import { DownloadService } from './services/download.service';
import {
    MessageService, NTSModalService, Configuration, Constants,
    NgbDateVNParserFormatter, MessageconfirmComponent, MessageComponent,
    FileProcess, ComboboxService, ComponentService, PermissionService
} from '../shared';
import { MessageconfirmcodeComponent } from './component/messageconfirmcode/messageconfirmcode.component';
import { ChooseFolderShareModalComponent } from './component/choose-folder-share-modal/choose-folder-share-modal.component';
import { ChooseFileModalComponent } from './component/choose-file-modal/choose-file-modal.component';
import { NTSDropDownTreeComponent } from './component/nts-drop-down-tree/nts-drop-down-tree.component';
import { NTSDropDownComponent } from './component/nts-drop-down/nts-drop-down.component';
import { NTSSearchBarComponent } from './component/nts-search-bar/nts-search-bar.component';
import { NtsTextMoreComponent } from './component/nts-text-more/nts-text-more.component';
import { ImportExcelComponent } from './component/import-excel/import-excel.component';
import { ScreenSaverComponent } from './component/screen-saver/screen-saver.component';
import { HistoryVersionComponent } from './component/history-version/history-version.component';
import { CommonModule } from '@angular/common';
import { SelectSkillComponent } from '../education/classroom/select-skill/select-skill/select-skill.component';
import { SelectPracticeComponent } from '../education/classroom/select-practice/select-practice.component';
import { NtsTinymceComponent } from './tinymce/nts-tinymce.component';
import { NtsStatusBadgeComponent } from './component/nts-status-badge/nts-status-badge.component';
import { NtsStatusComponent } from './component/nts-status/nts-status.component';
import { ImageViewerComponent } from './component/image-viewer/image-viewer.component';
import { NtsStatusTextComponent } from './component/nts-status-text/nts-status-text.component';
import { FilterErrorFixPipe } from './pipe/filter-error-fix.pipe';
// import { NgDragDropModule } from 'ng-drag-drop';
// import { AuthGuard } from '../auth/guards/auth.guard';

@NgModule({
    declarations: [
        TruncatePipe,
        FilterInListPipe,
        FilterBadgeClassInListPipe,
        FilterBadgeClassInListChildPipe,
        FilterTextClassInListPipe,
        UipermissionDirective,
        DiablePermissionDirective,
        ReadonlyPermissionDirective,
        NtscurrencyDirective,
        NtscurrencyPipe,
        NtsNumberDirective,
        PagingComponent,
        MessageconfirmComponent,
        MessageComponent,
        NtsNumberIntDirective,
        NtsLocationDirective,
        UisectionboxDirective,
        MessageconfirmcodeComponent,
        ChooseFolderShareModalComponent,
        NTSDropDownTreeComponent,
        NTSDropDownComponent,
        NTSSearchBarComponent,
        ImportExcelComponent,
        ScreenSaverComponent,
        ChooseFileModalComponent,
        HistoryVersionComponent,
        SelectSkillComponent,
        SelectPracticeComponent,
        NtsTinymceComponent,
        NtsStatusBadgeComponent,
        ImageViewerComponent, 
        NtsStatusComponent,
        NtsStatusTextComponent,
        FilterErrorFixPipe,
        NtsTextMoreComponent
    ],
    imports: [
        DevExtremeModule,
        DxTreeListModule,
        DxTreeViewModule,
        DxDropDownBoxModule,
        DxContextMenuModule,
        DxDateBoxModule,
        DxDataGridModule,
        FormsModule,
        CommonModule,
        NgbModule,
        CurrencyMaskModule,
        ToastrModule,
        VirtualScrollerModule
    ],
    providers: [
        //     MessageService,
        //     NTSModalService,
        // Configuration,
        // AppSetting,
        // Constants,
        // DateUtils,
        // FileProcess,
        // DownloadService,
        // ComboboxService,
        // ComponentService,
        // PermissionService,
        // NgbActiveModal,
        { provide: NgbDateParserFormatter, useClass: NgbDateVNParserFormatter },
        // AuthGuard,
        // {
        //   provide: APP_INITIALIZER,
        //   useFactory: initializeApp,
        //   deps: [Configuration],
        //   multi: true
        // },

    ],
    entryComponents: [
        MessageconfirmComponent,
        MessageComponent,
        MessageconfirmcodeComponent,
        ChooseFolderShareModalComponent,
        ImportExcelComponent,
        ChooseFileModalComponent,
        HistoryVersionComponent
    ],
    exports: [
        TruncatePipe,
        FilterInListPipe,
        FilterBadgeClassInListPipe,
        FilterBadgeClassInListChildPipe,
        FilterTextClassInListPipe,
        NtscurrencyPipe,
        UipermissionDirective,
        DiablePermissionDirective,
        NtscurrencyDirective,
        NtsNumberDirective,
        PagingComponent,
        MessageconfirmComponent,
        MessageComponent,
        NtsNumberIntDirective,
        NtsLocationDirective,
        UisectionboxDirective,
        NTSDropDownTreeComponent,
        NTSDropDownComponent,
        NTSSearchBarComponent,
        ImportExcelComponent,
        ReadonlyPermissionDirective,
        HistoryVersionComponent,
        SelectSkillComponent,
        SelectPracticeComponent,
        ScreenSaverComponent,
        NtsTinymceComponent,
        NtsStatusBadgeComponent,
        ImageViewerComponent,
        NtsStatusComponent,
        NtsStatusTextComponent,
        FilterErrorFixPipe,
        NtsTextMoreComponent,
    ]
})

export class SharedModule {}