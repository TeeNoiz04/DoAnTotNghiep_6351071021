import js from '@eslint/js';
import tsParser from '@typescript-eslint/parser';
import tsPlugin from '@typescript-eslint/eslint-plugin';
import angularPlugin from '@angular-eslint/eslint-plugin';
import angularTemplatePlugin from '@angular-eslint/eslint-plugin-template';
import angularTemplateParser from '@angular-eslint/template-parser';
import prettierPlugin from 'eslint-plugin-prettier';
import prettierConfig from 'eslint-config-prettier';

export default [
    // Ignore patterns
    {
        ignores: ['projects/**/*'],
    },

    // Base configuration for TypeScript files
    {
        files: ['**/*.ts'],
        languageOptions: {
            parser: tsParser,
            parserOptions: {
                project: ['tsconfig.json'],
                createDefaultProgram: true,
            },
        },
        plugins: {
            '@typescript-eslint': tsPlugin,
            '@angular-eslint': angularPlugin,
            'prettier': prettierPlugin,
        },
        rules: {
            // Base ESLint recommended rules
            ...js.configs.recommended.rules,
            // TypeScript ESLint recommended rules
            ...tsPlugin.configs.recommended.rules,
            // Prettier config (disable conflicting rules)
            ...prettierConfig.rules,

            // Turn off base rules that conflict with TypeScript
            'no-unused-vars': 'off',
            'no-undef': 'off',

            // TypeScript ESLint rules
            '@typescript-eslint/no-unused-vars': 'warn',
            '@typescript-eslint/no-explicit-any': 'warn',
            '@typescript-eslint/explicit-function-return-type': 'off',

            // Angular ESLint rules (recommended)
            ...angularPlugin.configs.recommended.rules,

            '@angular-eslint/component-selector': [
                'error',
                {
                    type: 'element',
                    prefix: 'app',
                    style: 'kebab-case',
                },
            ],

            // Prettier rules
            'prettier/prettier': 'error',
        },
    },

    {
        files: ['**/*.ts'],
        ignores: ["**/table/**", "**/columns/**"],
        languageOptions: {
            parser: tsParser,
            parserOptions: {
                project: ['tsconfig.json'],
                createDefaultProgram: true,
            },
        },
        plugins: {
            '@typescript-eslint': tsPlugin,
            '@angular-eslint': angularPlugin,
            'prettier': prettierPlugin,
        },
        rules: {
            // Angular component/directive selector rules
            '@angular-eslint/directive-selector': [
                'error',
                {
                    type: 'attribute',
                    prefix: 'app',
                    style: 'camelCase',
                },
            ],
        },
    },

    {
        files: ["**/*column*.directive.ts"],
        languageOptions: {
            parser: tsParser,
            parserOptions: {
                project: ['tsconfig.json'],
                createDefaultProgram: true,
            },
        },
        plugins: {
            '@typescript-eslint': tsPlugin,
            '@angular-eslint': angularPlugin,
            'prettier': prettierPlugin,
        },
        rules: {
            // Angular component/directive selector rules
            '@angular-eslint/directive-selector': [
                'error',
                {
                    type: 'element',
                    prefix: 'app',
                    style: 'kebab-case',
                },
            ],
        },
    },

    // Configuration for HTML template files
    {
        files: ['**/*.html'],
        languageOptions: {
            parser: angularTemplateParser,
        },
        plugins: {
            '@angular-eslint/template': angularTemplatePlugin,
        },
        rules: {
            // Angular template recommended rules
            ...angularTemplatePlugin.configs.recommended.rules,
            // Process inline templates
            ...angularTemplatePlugin.configs['process-inline-templates'].rules,
        },
    },
];