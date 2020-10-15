import { HttpEventType } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Game } from 'src/app/Models/Game';
import { GameInterestService } from 'src/app/Services/game-interest.service';
import { ToastifyService } from 'src/app/Services/toastify.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  @ViewChild('closeModal') closeModal;

  Games: Game[]
  gameToAdd: (any) = {};
  gameToEdit: (any) = {};

  gameIdToEdit = null;

  selectedFile = null;
  selectedFileToEdit = null;

  isAdding: boolean = false;
  isEditing: boolean = false;
  isDeleting: boolean = false;

  constructor(private gameService: GameInterestService, private toastify: ToastifyService) { }

  ngOnInit(): void {
    this.gameService.getGames().subscribe(games => this.Games = games)
  }

  isAddButtonDisabled() {
    return this.gameToAdd.name === undefined || this.gameToAdd.name === '';
  }

  isEditButtonDisabled() {
    return this.gameIdToEdit === null || this.gameToEdit.name === undefined || this.gameToEdit.name === '';
  }

  addGame() {
    if (this.gameToAdd) {
      this.isAdding = true;
      this.gameService.addGame(this.gameToAdd).subscribe(next => {
        this.Games.push(next);
        if (this.selectedFile) {
          const fd = new FormData();
          fd.append('File', this.selectedFile);
          this.gameService.uploadGameImage(next.gameId, fd).subscribe(events => {
            if (events.type === HttpEventType.UploadProgress) {
              console.log('Upload Progress: ' + Math.round(events.loaded / events.total * 100));
              if (events.total === events.loaded)
                this.isAdding = false;
              this.toastify.show('You add game successfully');
            }
            this.selectedFile = null;
          })
        } else {
          this.isAdding = false;
          this.toastify.show('You add game successfully');
        }
        this.gameToAdd = {};
      });
    }
  }

  saveEdit() {
    if (this.gameToEdit) {
      this.isEditing = true;
      this.gameService.editGame(this.gameToEdit.gameId, this.gameToEdit).subscribe(next => {
        this.gameToEdit.name = next.name;
        if (this.selectedFileToEdit) {
          const fd = new FormData();
          fd.append('File', this.selectedFileToEdit);
          this.gameService.uploadGameImage(next.gameId, fd).subscribe(events => {
            if (events.type === HttpEventType.UploadProgress) {
              console.log('Upload Progress: ' + Math.round(events.loaded / events.total * 100));
              if (events.total === events.loaded)
                this.isEditing = false;
              this.toastify.show('You edit game successfully');
            }
            this.selectedFileToEdit = null;
          })
        } else {
          this.isEditing = false;
          this.toastify.show('You edit game successfully');
        }
      });
    }
  }

  onDeleteGame() {
    this.isDeleting = true;
    if (this.gameIdToEdit)
      this.gameService.deleteGame(this.gameIdToEdit).subscribe(next => {
        this.Games = this.Games.filter(x => x.gameId !== next.gameId);
        this.closeModal.nativeElement.click();
        this.toastify.show('Delete success!')
        this.isDeleting = false;
      }, error => this.isDeleting = false);
  }

  onSelectedGame() {
    if (this.gameIdToEdit) {
      const game = this.Games.find(x => x.gameId.toString() === this.gameIdToEdit);
      this.gameToEdit = game;
    }
  }

  onSelectFile(event) {
    if (event.target.files.length > 0) {
      this.selectedFile = event.target.files[0];
    } else {
      this.selectedFile = null;
    }
  }

  onSelectFileToEdit(event) {
    if (event.target.files.length > 0) {
      this.selectedFileToEdit = event.target.files[0];
    } else {
      this.selectedFileToEdit = null;
    }
  }

}
