export interface ImageViewerConfig {
    btnClass?: string;
    zoomFactor?: number;
    containerBackgroundColor?: string;
    wheelZoom?: boolean;
    allowFullscreen?: boolean;
    allowKeyboardNavigation?: boolean;

    btnShow?: {
      save?: boolean,
      zoomIn?: boolean;
      zoomOut?: boolean;
      rotateClockwise?: boolean;
      rotateCounterClockwise?: boolean;
      next?: boolean;
      prev?: boolean;
    };

    btnIcons?: {
        save?: string;
        zoomIn?: string;
        zoomOut?: string;
        rotateClockwise?: string;
        rotateCounterClockwise?: string;
        next?: string;
        prev?: string;
        fullscreen?: string;
    };

    customBtns?: Array<
      {
        name: string;
        icon: string;
      }
    >;
}

export class CustomEvent {
  name: string;
  imageIndex: number;

  constructor(name, imageIndex) {
    this.name = name;
    this.imageIndex = imageIndex;
  }
}
