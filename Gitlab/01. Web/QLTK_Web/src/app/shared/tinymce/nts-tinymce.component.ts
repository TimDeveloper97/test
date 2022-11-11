import { Component, OnDestroy, AfterViewInit, forwardRef, NgZone, Inject, Output, EventEmitter, Input, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { NtsTinymceDefaultOptions } from './nts-tinymce.default';
import { NtsTinymceOptions } from './nts-tinymce.config.interface';

import 'src/assets/tinymce/tinymce.min';
declare var tinymce: any;

import 'src/assets/tinymce/themes/modern/theme';
import 'src/assets/tinymce/plugins/link/plugin.js';
import 'src/assets/tinymce/plugins/paste/plugin.js';
import 'src/assets/tinymce/plugins/table/plugin.js';
import 'src/assets/tinymce/plugins/advlist/plugin.js';
import 'src/assets/tinymce/plugins/autoresize/plugin.js';
import 'src/assets/tinymce/plugins/lists/plugin.js';
import 'src/assets/tinymce/plugins/code/plugin.js';

const noop = () => {
};

@Component({
	selector: 'nts-app-tinymce',
	template: '<div id="{{elementId}}"></div>',
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => NtsTinymceComponent),
			multi: true
		}
	]
})

export class NtsTinymceComponent implements ControlValueAccessor, AfterViewInit, OnDestroy, OnInit {
	public elementId: string = 'tiny-' + Math.random().toString(36).substring(2);
	public editor: any;
	@Output() focus = new EventEmitter();
	@Input() config: any;
	@Input() isCenter: boolean;
	private onTouchedCallback: () => void = noop;
	private onChangeCallback: (_: any) => void = noop;
	private innerValue: string;

	private options: any;
	constructor(
		private zone: NgZone,

	) {

	}

	ngOnInit() {
		this.options = Object.assign(new NtsTinymceDefaultOptions(), this.config);
		const iscenter = this.isCenter;
		this.options.selector = '#' + this.elementId;
		this.options.setup = editor => {
			this.editor = editor;
			editor.on('change keyup', () => {
				const content = editor.getContent();
				this.value = content;
			});
			editor.on('focus', e => {
				this.focus.emit(e);
			});
			editor.on('ResizeEditor', function (e) {

				// do whatever you need here
			});
			editor.on('init', e => {
				if (iscenter === true) {
					const content = editor.getContent();
					if (!content.includes("text-align: center")) {
						setTimeout(function () {
							editor.execCommand('JustifyCenter');
						}, 1);
					}
				}
			});

			if (typeof this.config.setup === 'function') {
				this.config.setup(editor);
			}
		}
		this.options.init_instance_callback = editor => {
			editor && this.value && editor.setContent(this.value)
			if (typeof this.config.init_instance_callback === 'function') {
				this.config.init_instance_callback(editor);
			}
		}
		if (this.config.auto_focus) {
			this.options.auto_focus = this.elementId;
		}
	}


	ngAfterViewInit() {
		if (this.options.baseURL) {
			tinymce.baseURL = this.options.baseURL;
		}
		tinymce.init(this.options);
	}

	ngOnDestroy() {
		tinymce.remove(this.editor);
	}

	// get accessor
	get value(): any {
		return this.innerValue;
	};

	// set accessor including call the onchange callback
	set value(v: any) {
		if (v !== this.innerValue) {
			this.innerValue = v;
			this.zone.run(() => {
				this.onChangeCallback(v);
			});

		}
	}
	// From ControlValueAccessor interface
	writeValue(value: any) {
		if (value !== this.innerValue) {
			this.innerValue = value;
			if (!value) {
				value = '';
			}
			this.editor && this.editor.setContent(value);
		}
	}

	registerOnChange(fn: any) {
		this.onChangeCallback = fn;
	}

	registerOnTouched(fn: any) {
		this.onTouchedCallback = fn;
	}
}