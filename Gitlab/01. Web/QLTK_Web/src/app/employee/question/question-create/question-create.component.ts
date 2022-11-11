import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Configuration, Constants, FileProcess, MessageService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { QuestionService } from '../../service/question.service';
declare var tinymce: any;
import { forkJoin } from 'rxjs';
@Component({
  selector: 'app-question-create',
  templateUrl: './question-create.component.html',
  styleUrls: ['./question-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class QuestionCreateComponent implements OnInit {

  discoveryConfig = {
    plugins: ['image code', 'visualblocks', 'print preview', 'table', 'directionality', 'link', 'media', 'codesample', 'table', 'charmap', 'hr', 'pagebreak', 'nonbreaking', 'anchor', 'toc', 'insertdatetime', 'advlist', 'lists', 'textcolor', 'wordcount', 'imagetools', 'contextmenu', 'textpattern', 'searchreplace visualblocks code fullscreen',],
    language: 'vi_VN',
    // file_picker_types: 'file image media',
    automatic_uploads: true,
    toolbar: 'undo redo | fontselect | fontsizeselect | bold italic forecolor backcolor |alignleft aligncenter alignright alignjustify alignnone | numlist | table | link | outdent indent',
    convert_urls: false,
    height: 200,
    auto_focus: false,
    plugin_preview_width: 1000,
    plugin_preview_height: 650,
    readonly: 0,
    content_style: "body {font-size: 12pt;font-family: Arial;}",
    file_browser_callback: function RoxyFileBrowser(field_name, url, type, win) {
      //var roxyFileman = '/fileman/index.html';
      var roxyFileman = "https://nhantinsoft.vn:9508/fileServer/fileman/index.html";
      if (roxyFileman.indexOf("?") < 0) {
        roxyFileman += "?type=" + type;
      }
      else {
        roxyFileman += "&type=" + type;
      }
      roxyFileman += '&input=' + field_name + '&value=' + win.document.getElementById(field_name).value;
      if (tinymce.activeEditor.settings.language) {
        roxyFileman += '&langCode=' + tinymce.activeEditor.settings.language;
      }
      tinymce.activeEditor.windowManager.open({
        file: roxyFileman,
        title: 'Roxy Fileman',
        width: 850,
        height: 650,
        resizable: "yes",
        plugins: "media",
        inline: "yes",
        close_previous: "no"
      }, {
        window: win,
        input: field_name
      });
      return false;
    },
    //setup: TinymceUserConfig.setup,
    // content_css: '/assets/css/custom_editor.css',
    images_upload_handler: (blobInfo, success, failure) => {
      this.fileService.uploadFile(blobInfo.blob(), 'Email-Configuration').subscribe(
        result => {
          success(this.config.ServerApi + result.data.fileUrl);
        },
        error => {
          return;
        }
      );
    }
  };

  isAction: boolean = false;
  id: string = '';
  groupId: string;
  modalInfo = {
    Title: 'Thêm mới câu hỏi',
    SaveText: 'Lưu',
  };

  questionGroups: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  questionModel: any = {
    Id: '',
    QuestionGroupId: '',
    Code: '',
    Type: 1,
    Score: '',
    Content: '',
    Answer: '',
    QuestionFiles: []
  };

  answerTrueFalse: string = '1';
  answer: string = '';
  isView: boolean = false;

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public config: Configuration,
    private fileService: UploadfileService,
    private questionService: QuestionService,
    private comboboxService: ComboboxService,
    public fileProcess: FileProcess,) { }

  ngOnInit(): void {
    this.getCbbQuestionGroup();
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa câu hỏi';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.questionModel.QuestionGroupId = this.groupId;
      this.modalInfo.Title = 'Thêm mới câu hỏi';
    }
  }

  getCbbQuestionGroup() {
    this.comboboxService.getQuestionGroup().subscribe(data => {
      this.questionGroups = data;
    }, error => {
      this.messageService.showError(error);
    })
  }

  getInfo() {
    this.questionService.getInfoQuestion({ Id: this.id }).subscribe(
      result => {
        this.questionModel = result;
        if (this.questionModel.Type == 1) {
          this.answerTrueFalse = this.questionModel.Answer;
        } else if (this.questionModel.Type == 2) {
          this.answer = this.questionModel.Answer;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.questionService.createQuestion(this.questionModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới câu hỏi thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới câu hỏi thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.questionService.updateQuestion(this.questionModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật câu hỏi thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.questionModel.Type == 1) {
      this.questionModel.Answer = this.answerTrueFalse;
    } else if (this.questionModel.Type == 2) {
      this.questionModel.Answer = this.answer;
    }

    if (this.documentFilesUpload.length > 0) {
      let questionFiles = this.fileService.uploadListFile(this.documentFilesUpload, 'QuestionFiles/');
      forkJoin([questionFiles]).subscribe(results => {
        if (results[0].length > 0) {
          results[0].forEach(item => {
            var questionFile = this.questionModel.QuestionFiles.find(a => a.FileName == item.FileName && a.FileSize == item.FileSize);
            questionFile.FilePath = item.FileUrl;
          });
        }
        if (this.id) {
          this.update();
        }
        else {
          this.create(isContinue);
        }
      });
    } else {
      if (this.id) {
        this.update();
      }
      else {
        this.create(isContinue);
      }
    }
  }

  downloadAFile(row: any) {
    this.fileProcess.downloadFileBlob(row.FilePath, row.FileName);
  }

  deleteFile(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file này không?").then(
      data => {
        if (this.questionModel.QuestionFiles.length > 0) {
          this.questionModel.QuestionFiles.splice(index, 1);
        }
      },
      error => {
      }
    );
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  fileModel: any = {
    FileName: '',
    FilePath: '',
    FileSize: null,
    CreateBy: null,
    CreateDate: null
  };
  documentFilesUpload: any = [];
  uploadFileClick($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var filesExist = "";

    for (var item of fileDataSheet) {
      var file = Object.assign({}, this.fileModel);
      file.FileName = item.name;
      file.FileSize = item.size;
      file.CreateDate = new Date();

      var fileExist = this.questionModel.QuestionFiles.find(a => a.FileName == item.name && a.FileSize == item.size);
      if (fileExist != null) {
        filesExist = fileExist.FileName + ", " + filesExist;
      } else {
        this.documentFilesUpload.push(item);
        this.questionModel.QuestionFiles.push(file);
      }
    }
    if (filesExist != "") {
      this.messageService.showMessage("File: " + filesExist + " đã tồn tại");
    }
  }


  clear() {
    this.questionModel = {
      Id: '',
      QuestionGroupId: '',
      Code: '',
      Type: 1,
      Score: '',
      Content: '',
      Answer: '',
      QuestionFiles: []
    };
  }

}
