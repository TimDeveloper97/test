import { Injectable } from '@angular/core';
import { FileRepositoryService } from './file-repository.service';
import { UserRepositoryService } from './user-repository.service';

@Injectable({
  providedIn: 'root'
})
export class AppRepositoryService {

  constructor(
    private fileRepository: FileRepositoryService,
    private userRepository: UserRepositoryService
  ) {

  }
  get File(): FileRepositoryService {
    return this.fileRepository;
  }
  get User(): UserRepositoryService {
    return this.userRepository;
  }
}
